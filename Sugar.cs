using Godot;

namespace ludumdare56;

public partial class Sugar : Area3D, ITouchable
{
    public void GetTouched(BacteriumArea toucher)
    {
        toucher.Score(1);
        GetEaten();
    }

    public void GetEaten()
    {
        Visible = false;
    }
}