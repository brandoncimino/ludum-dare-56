using Godot;
using System;

public partial class Cell : Node3D
{
	private Transform3D transform;
	[Export()] private float speed = 1;

	public Cell()
	{
		transform = this.GetTransform();
		
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		apply_movement((float) delta);
	}

	private void apply_movement(float delta)
	{
		// get current position
		Vector3 position = this.Position;
		
		// move downwards
		position += Vector3.Down * (speed * delta);
		
		// check for domain boundary and adjust
		position = apply_boundary_conditions(position);
		
		// apply changes in the position
		this.Position = position;
	}

	private Vector3 apply_boundary_conditions(Vector3 position)
	{
		
		// check if below bottom edge of screen
		// bottom edge of screen is at y=-50, using an extra 10% buffer
		if (position.Y < -55)
		{
			// place object above the top of the screen
			position.Y = 55;
		}

		return (position);
	}

	
	
	
}
