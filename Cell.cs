using System.Linq;
using Godot;
using ludumdare56;

public partial class Cell : Area3D, ITouchable
{
    private FlowParticle? _parent;
    private FlowParticle Parent => _parent ??= this.EnumerateParents().OfType<FlowParticle>().First();

    public void GetTouched(BacteriumArea toucher)
    {
        toucher.IncreaseHeat(1);
        Parent.QueueFree();
    }
}