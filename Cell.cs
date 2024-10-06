using Godot;
using System;
using ludumdare56;
using Timer = Godot.Timer;

public partial class Cell : Node3D
{
	// cell properties
	[Export()] private float rotation_speed = 5;
	private Vector3 rotation_axis = Vector3.Zero;
	
	[Export()] private float movement_speed = 5;
	private Vector3 tumbling_direction = Vector3.Zero;
	
	// interaction with environment
	[Export()] private CellManager manager;
	private Timer timer_for_changing_movement;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		on_timeout_for_change_movement();
	}

	public void Initialize(CellManager manager, Vector3 spawn_position)
	{
		manager = manager;
		this.Position = spawn_position;
		
		timer_for_changing_movement = new Timer();
		AddChild(timer_for_changing_movement);
		timer_for_changing_movement.WaitTime = 3; //randomizer.RandfRange(1, 5); // in seconds
		timer_for_changing_movement.OneShot = false;
		timer_for_changing_movement.Timeout += on_timeout_for_change_movement;
		timer_for_changing_movement.Autostart = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		apply_movement((float) delta);
	}

	private void apply_movement(float delta)
	{
		// apply rotation
		Rotate(rotation_axis, delta * 5);
		
		// get current position
		Vector3 position = this.Position;
		
		// move downwards
		position += Vector3.Down * (movement_speed * delta);
		
		// move in tumbling direction
		position += tumbling_direction * (movement_speed * delta);
		
		// check for domain boundary and adjust
		position = apply_boundary_conditions(position);
		
		// apply changes in the position
		this.Position = position;
	}

	private Vector3 apply_boundary_conditions(Vector3 position)
	{
		
		// check if below bottom edge of screen
		// bottom edge of screen is at y=-50, using an extra 10% buffer
		if (position.Y < 1.1 * GameManager.boundary_bot)
		{
			// place object above the top of the screen
			position.Y = 1.1f * GameManager.boundary_top;
		}

		// check for right side of the domain
		if (position.X > GameManager.boundary_right)
		{
			// reposition back into the domain and reverse tumbling direction
			position.X = GameManager.boundary_right;
			tumbling_direction.X *= -1;
		}
		
		// check for left side of the domain
		if (position.X < GameManager.boundary_left)
		{
			// reposition back into the domain and reverse tumbling direction
			position.X = GameManager.boundary_left;
			tumbling_direction.X *= -1;
		}
		
		return (position);
	}

	private void on_timeout_for_change_movement()
	{
		RandomNumberGenerator randomizer = new RandomNumberGenerator();
		
		// vector around we rotate
		var adjustment_rotation = new Vector3(
			randomizer.Randfn(0, 1f), 
			randomizer.Randfn(0, 2), 
			randomizer.Randfn(0, 0.5f)
		);
		adjustment_rotation = adjustment_rotation.Normalized();
		
		// take mean with previous direction to avoid too sudden changes
		rotation_axis += 0.5f * adjustment_rotation;
		rotation_axis = rotation_axis.Normalized();
		
		// direction in which we tumble
		var adjustment_direction = new Vector3(
			randomizer.Randfn(0, 1f), 
			randomizer.Randfn(0, 0.5f), 
			randomizer.Randfn(0, 1f)
		);
		// take mean with previous direction to avoid too sudden changes
		tumbling_direction = (3 * tumbling_direction + adjustment_direction) / 4;
		tumbling_direction.Y = Mathf.Min(tumbling_direction.Y, -0.5f);
		tumbling_direction.Z = 0f;
		// TODO: activate or deactivate based on distance to Z=0

		timer_for_changing_movement.WaitTime = randomizer.RandfRange(1, 5); // in seconds;
		timer_for_changing_movement.Start();
	}

	
	
	
}
