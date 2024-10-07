using Godot;

public partial class WantedStar : TextureRect
{
    [Export] private Texture2D _filledTexture;
    private Texture2D _emptyTexture;

    public bool IsFilled { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _emptyTexture = Texture;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void SetFilled(bool filled)
    {
        IsFilled = filled;
        Texture = filled ? _filledTexture : _emptyTexture;
    }
}