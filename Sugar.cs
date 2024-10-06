using Godot;

namespace ludumdare56;

public partial class Sugar : FlowParticle, ITouchable
{
    public void GetTouched(BacteriumArea toucher)
    {
        GetEaten();
    }

    public void GetEaten()
    {
        Visible = false;
    }
}