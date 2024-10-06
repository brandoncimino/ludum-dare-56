using Godot;
using System;
using ludumdare56;
using Vector3 = System.Numerics.Vector3;

public partial class Bacterium : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		SnapToMouse();
	}

	private void SnapToMouse()
	{
		GlobalPosition = GetViewport().GetCamera3D()
			.ScreenToWorldPosition(GetViewport().GetMousePosition().Clamp(default, GetWindow().Size)).WithZ(0);
	}
}
