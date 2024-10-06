using Godot;
using System;
using System.Collections.Generic;
using ludumdare56;
using Timer = Godot.Timer;

public partial class FlowManager : Node
{
	private RandomNumberGenerator randomizer;
	[Export] public PackedScene CellScene { get; set; }
	private List<Node> list_cells;

	private Timer timer_for_next_cell_spawn;
	
	public FlowManager()
	{
		randomizer = new RandomNumberGenerator();
		list_cells = new List<Node>();
		
		timer_for_next_cell_spawn = new Timer();
		AddChild(timer_for_next_cell_spawn);
		timer_for_next_cell_spawn.WaitTime = randomizer.RandfRange(1, 5); // in seconds
		timer_for_next_cell_spawn.OneShot = true;
		timer_for_next_cell_spawn.Timeout += on_timeout_for_next_cell_spawn;
		timer_for_next_cell_spawn.Autostart = true;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// populate the world with some cells to start out
		for (var i = 0; i < 10; i++)
		{
			spawn_a_cell(bool_spawn_at_top: false);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void spawn_a_cell(bool bool_spawn_at_top)
	{
		// Create new instance of cell scene
		Cell cell = CellScene.Instantiate<Cell>();
		
		// generate random spawn position in 2D world
		Vector3 spawn_position = new Vector3(
			randomizer.RandfRange(GameManager.boundary_left, GameManager.boundary_right),
			randomizer.RandfRange(GameManager.boundary_bot, GameManager.boundary_top),
			0
		);
		
		// if supposed to spawn at the top, adjust the y-position
		if (bool_spawn_at_top)
		{
			spawn_position.Y = GameManager.boundary_top * 1.1f;
		}

		// initialize the cell
		cell.Initialize(this, spawn_position);
			
		// Spawn the cell by adding it to the Main scene.
		AddChild(cell);
		
		// remember this cell in case we need it in the future
		list_cells.Add(cell);
	}

	private void on_timeout_for_next_cell_spawn()
	{
		// spawn in a new cell
		if (list_cells.Count < 200)
		{
			spawn_a_cell(true);
		}
		
		// restart countdown from random time
		timer_for_next_cell_spawn.WaitTime = randomizer.RandfRange(1, 5); // in seconds
		timer_for_next_cell_spawn.Start();
	}
}
