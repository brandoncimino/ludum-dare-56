using System;
using Godot;

namespace ludumdare56;

public partial class Neutrophil : Area3D, ITouchable
{
    public void GetTouched(BacteriumArea toucher)
    {
        throw new NotImplementedException();
    }
}