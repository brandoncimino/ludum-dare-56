using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Godot;
using JetBrains.Annotations;
using ludumdare56;

public partial class TheFuzz : Control
{
    public const int MaxWantedLevel = 5;
    private static TheFuzz? _instance;

    public static TheFuzz GetInstance() =>
        _instance ?? throw new InvalidOperationException($"{typeof(TheFuzz)} doesn't exist!");

    public double Heat { get; private set; }

    public int WantedLevel => Math.Clamp((int)Heat, 0, MaxWantedLevel);

    private TimeSpan _timeSinceLastHeat;

    private TimeSpan HeatCooldown =>
        WantedLevel == 0 ? TimeSpan.Zero : Math.Pow(WantedLevel, 1.5) * TimeSpan.FromSeconds(1);

    [Export] private PackedScene? WantedStarScene;

    private HBoxContainer? _wantedStarContainer;

    private HBoxContainer WantedStarContainer =>
        _wantedStarContainer ??= this.GetChildren().OfType<HBoxContainer>().Single();

    private ImmutableArray<WantedStar> _wantedStars;
    private ProgressBar? _exactHeatBar;
    private ProgressBar ExactHeatBar => _exactHeatBar ??= this.EnumerateChildren().OfType<ProgressBar>().Single();
    private Label? _debugLabel;
    private Label DebugLabel => _debugLabel ??= this.EnumerateChildren().OfType<Label>().Single();

    private bool IsCoolingOff => _timeSinceLastHeat >= HeatCooldown;

    /// <summary>
    /// <code><![CDATA[
    /// ⭐ 🧊
    /// -- --
    ///  0  ♾
    ///  1  .5/1 => .5
    ///  2  .5/2 => .25
    ///  3	.5/3 => .166
    ///  4  .5/4 => .125
    /// ]]></code> 
    /// </summary>
    private double CoolingPerSecond => .5 / (WantedLevel + 1);

    [MustUseReturnValue]
    private static ImmutableArray<WantedStar> PopulateWantedStars(HBoxContainer wantedStarContainer,
        PackedScene starScene, int maxWantedLevel)
    {
        ArgumentNullException.ThrowIfNull(starScene);

        var array = ImmutableArray.CreateBuilder<WantedStar>(maxWantedLevel);
        for (int i = 0; i < maxWantedLevel; i++)
        {
            var wantedStar = starScene.Instantiate<WantedStar>();
            wantedStarContainer.AddChild(wantedStar);
            array.Add(wantedStar);
        }

        return array.MoveToImmutable();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (_instance is null)
        {
            _instance = this;
        }
        else
        {
            throw new InvalidOperationException($"{typeof(TheFuzz)} is already here!");
        }

        _wantedStars = PopulateWantedStars(WantedStarContainer, WantedStarScene!, MaxWantedLevel);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Space))
        {
            IncreaseHeat(delta * 2);
        }

        ProcessCooldown(delta);
        UpdateWantedStarDisplay();
    }

    private void UpdateWantedStarDisplay()
    {
        ExactHeatBar.FillMode = (int)ProgressBar.FillModeEnum.EndToBegin;
        ExactHeatBar.MaxValue = MaxWantedLevel;
        ExactHeatBar.Value = Heat;

        for (int i = 0; i < _wantedStars.Length; i++)
        {
            if (IsCoolingOff && i == WantedLevel - 1)
            {
                _wantedStars[i].SetFilled(
                    Blink(TimeSpan.FromSeconds(.25), _timeSinceLastHeat, true)
                );
            }
            else
            {
                _wantedStars[i].SetFilled(i < WantedLevel);
            }
        }

        DebugLabel.Text = $"""
                           Heat:             {Heat}
                           HeatCooldown:     {HeatCooldown}
                           since last heat:  {_timeSinceLastHeat}
                           """;
    }

    private static bool Blink(TimeSpan phaseDuration, TimeSpan currentTime, bool initialState)
    {
        var toggleCount = (int)currentTime.Divide(phaseDuration);
        var matchInitialState = toggleCount % 2 == 0;
        return matchInitialState ? initialState : !initialState;
    }

    private void ProcessCooldown(double delta)
    {
        _timeSinceLastHeat += TimeSpan.FromSeconds(delta);
        if (IsCoolingOff)
        {
            DecreaseHeat(CoolingPerSecond * delta);
        }
    }

    public void IncreaseHeat(double heat)
    {
        Debug.Assert(heat > 0);

        _timeSinceLastHeat = TimeSpan.Zero;
        Heat = Math.Min(Heat + heat, MaxWantedLevel + 1);
    }

    public void DecreaseHeat(double heat)
    {
        Debug.Assert(heat > 0);

        Heat = Math.Max(0, Heat - heat);
    }
}