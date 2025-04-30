using System;
using Godot;

namespace ludumdare56;

public partial class WhiteBloodCellArea : Area3D, ITouchable
{
    [Export] public WhiteBloodCellRank Rank { get; set; }

    public void GetTouched(BacteriumArea toucher)
    {
        throw new NotImplementedException();
    }
}