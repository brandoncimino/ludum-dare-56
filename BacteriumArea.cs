using System;
using System.Linq;
using Godot;
using ludumdare56;

public partial class BacteriumArea : Area3D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var overlaps = GetOverlappingAreas();
        foreach (var touchable in overlaps.OfType<ITouchable>())
        {
            Console.WriteLine($"{this} TOUCHED {touchable}");
            touchable.GetTouched(this);
        }
    }

    public void IncreaseHeat(double amount)
    {
        TheFuzz.GetInstance().IncreaseHeat(amount);
    }
}