using System;
using Godot;

namespace ludumdare56;

public partial class Sugar : Area3D, ITouchable
{
    [Export()] private FlowParticle parent;
    public void GetTouched(BacteriumArea toucher)
    {
        GetEaten();
    }

    public void GetEaten()
    {
        parent.QueueFree();
    }
    
}