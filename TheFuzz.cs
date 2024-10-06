using System;
using System.Diagnostics;
using Godot;

public partial class TheFuzz : Control
{
    public const int MaxWantedLevel = 5;
    private static TheFuzz? _instance;

    public static TheFuzz GetInstance() =>
        _instance ?? throw new InvalidOperationException($"{typeof(TheFuzz)} doesn't exist!");

    public double Heat { get; private set; }

    public int WantedLevel => Mathf.Min((int)Heat, MaxWantedLevel);

    private TimeSpan _timeSinceLastHeat;
    private TimeSpan HeatCooldown => Math.Pow(WantedLevel, 1.5) * TimeSpan.FromSeconds(1);

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
    private double CoolingPerSecond => WantedLevel == 0 ? double.PositiveInfinity : .5 / WantedLevel;

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
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        ProcessCooldown(delta);
    }

    private void ProcessCooldown(double delta)
    {
        if (_timeSinceLastHeat > HeatCooldown)
        {
            DecreaseHeat(CoolingPerSecond * delta);
        }
        else
        {
            _timeSinceLastHeat += TimeSpan.FromSeconds(delta);
        }
    }

    public void IncreaseHeat(double heat)
    {
        Debug.Assert(heat > 0);
        _timeSinceLastHeat = TimeSpan.Zero;
        Heat = Math.Min(Heat, MaxWantedLevel + 1);
    }

    public void DecreaseHeat(double heat)
    {
        Debug.Assert(heat > 0);
        Heat -= heat;
    }
}