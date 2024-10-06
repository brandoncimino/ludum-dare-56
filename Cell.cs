using Godot;
using ludumdare56;
using Timer = Godot.Timer;

public partial class Cell : FlowParticle, ITouchable
{
    public void GetTouched(BacteriumArea toucher)
    {
        toucher.IncreaseHeat(0.1);
    }
}