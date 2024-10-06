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
	
	[Export] public PackedScene SugarScene { get; set; }

	private Timer timer_for_next_particle_spawn;
	
	public FlowManager()
	{
		randomizer = new RandomNumberGenerator();
		list_cells = new List<Node>();
		
		timer_for_next_particle_spawn = new Timer();
		AddChild(timer_for_next_particle_spawn);
		timer_for_next_particle_spawn.WaitTime = randomizer.RandfRange(1, 5); // in seconds
		timer_for_next_particle_spawn.OneShot = true;
		timer_for_next_particle_spawn.Timeout += OnTimeoutForNextParticleSpawn;
		timer_for_next_particle_spawn.Autostart = true;
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

	private void spawn_a_particle(bool bool_spawn_at_top, FlowParticle particle)
	{
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
		particle.Initialize(this, spawn_position);
			
		// Spawn the cell by adding it to the Main scene.
		AddChild(particle);
	}
	private void spawn_a_cell(bool bool_spawn_at_top)
	{
		// Create new instance of cell scene
		Cell cell = CellScene.Instantiate<Cell>();
		
		// spawn into the world
		spawn_a_particle(bool_spawn_at_top, cell);
		
		// remember this cell in case we need it in the future
		list_cells.Add(cell);
	}
	
	private void spawn_a_sugar()
	{
		// Create new instance of cell scene
		FlowParticle sugar = SugarScene.Instantiate<FlowParticle>();
		
		// spawn into the world
		spawn_a_particle(true, sugar);
	}

	private void OnTimeoutForNextParticleSpawn()
	{
		var sample = randomizer.RandfRange(0, 1);
		if (sample < 0.5)
		{
			// spawn in a new cell
			if (list_cells.Count < 200)
			{
				spawn_a_cell(true);
			}
		}
		else
		{
			// spawn in a sugar
			spawn_a_sugar();
		}
		
		// restart countdown from random time
		timer_for_next_particle_spawn.WaitTime = randomizer.RandfRange(1, 5); // in seconds
		timer_for_next_particle_spawn.Start();
	}
}
