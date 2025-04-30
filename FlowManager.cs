using System;
using System.Collections.Generic;
using Godot;
using ludumdare56;
using Timer = Godot.Timer;

public partial class FlowManager : Node
{
    /// TODO: What are the pros/cons of using Godot's <see cref="RandomNumberGenerator"/> vs. C#'s <see cref="Random"/>? 
    private RandomNumberGenerator randomizer;

    /// TODO: What happens when we use <see cref="Node.QueueFree"/> to delete an object, but we still have a reference to it in C#?
    private readonly List<Node> list_cells;

    [Export] private PackedScene CellScene { get; set; }
    [Export] private PackedScene SugarScene { get; set; }
    [Export] private PackedScene NeutrophilScene { get; set; }

    private Timer timer_for_next_particle_spawn;

    private readonly List<SpawnTimer> _spawners = [];

    public FlowManager()
    {
        randomizer = new RandomNumberGenerator();
        list_cells = new List<Node>();

        timer_for_next_particle_spawn = new Timer();
        AddChild(timer_for_next_particle_spawn);
        timer_for_next_particle_spawn.WaitTime = randomizer.RandfRange(1, 5); // in seconds
        timer_for_next_particle_spawn.OneShot = true;
        // timer_for_next_particle_spawn.Timeout += OnTimeoutForNextParticleSpawn;
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

        // Start the repeating spawners for cells and sugar
        _spawners.Add(CreateSugarSpawner());
        _spawners.Add(CreateCellSpawner());
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _Process(double delta)
    {
        foreach (var spawner in _spawners)
        {
            spawner.AdvanceTime(delta.Seconds());
        }
    }

    private void spawn_a_particle(PackedScene scene, bool spawn_at_top = true)
    {
        var cell = scene.Instantiate<FlowParticle>();
        spawn_a_particle(spawn_at_top, cell);
        // TODO: 1) Decide if we need track the spawned particles, like we do with `list_cells`; 2) if we do, do it in a generic way; probably with a `Dictionary<PackedScene, List<FlowParticle>` - though we should be careful because "deleting" an instance of `FlowParticle` won't remove it from the list 
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
        FlowParticle cell = CellScene.Instantiate<FlowParticle>();

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

    private SpawnTimer CreateSugarSpawner()
    {
        return new SpawnTimer(
            stars =>
            {
                var lerpAmount = (float)stars / TheFuzz.MaxWantedLevel;
                const int longestInterval = 5;
                const int shortestInterval = 1;
                var center = Mathf.Lerp(longestInterval, shortestInterval, lerpAmount).Seconds();
                var radius = center / 10;
                return SpawnTimer.TimeRange.Radius(center, radius);
            },
            spawn_a_sugar
        );
    }

    private SpawnTimer CreateCellSpawner()
    {
        return new SpawnTimer(
            _ => new SpawnTimer.TimeRange(.2.Seconds(), .5.Seconds()),
            () => spawn_a_cell(true)
        );
    }

    private SpawnTimer CreateNeutrophilSpawner()
    {
        return new SpawnTimer(
            stars =>
            {
                var center = stars switch
                {
                    >= 1 => TimeSpan.FromSeconds(1f - .1f * stars),
                    _ => TimeSpan.MaxValue
                };

                var radius = center / 10;
                return SpawnTimer.TimeRange.Radius(center, radius);
            },
            () => spawn_a_particle(NeutrophilScene)
        );
    }
}