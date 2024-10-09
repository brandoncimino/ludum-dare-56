using System;
using Godot;

namespace ludumdare56;

public sealed class SpawnTimer
{
    private Seconds _timeSinceLastSpawn;
    private Seconds _nextSpawnInterval;
    private readonly Random _random = Random.Shared;

    /// <summary>
    /// Takes in the <see cref="TheFuzz.WantedLevel"/> and produces a range of possible <see cref="TimeSpan"/>s.
    /// </summary>
    public Func<int, TimeRange> SpawnInterval_ByWantedLevel { get; }

    /// <summary>
    /// The code that gets executed when the <see cref="_nextSpawnInterval"/> elapses.
    /// </summary>
    public Action OnSpawn { get; }

    public SpawnTimer(Func<int, TimeRange> spawnIntervalByWantedLevel, Action onSpawn)
    {
        SpawnInterval_ByWantedLevel = spawnIntervalByWantedLevel;
        OnSpawn = onSpawn;
    }

    private Seconds GetNextSpawnInterval()
    {
        var range = SpawnInterval_ByWantedLevel(TheFuzz.GetInstance().WantedLevel);
        return Mathf.Lerp(range.Min.TotalSeconds, range.Max.TotalSeconds, _random.NextDouble()).Seconds();
    }

    private void Spawn()
    {
        _timeSinceLastSpawn = default;
        _nextSpawnInterval = GetNextSpawnInterval();
        OnSpawn();
    }

    public void AdvanceTime(Seconds elapsed)
    {
        _timeSinceLastSpawn += elapsed;

        if (_timeSinceLastSpawn >= _nextSpawnInterval)
        {
            Spawn();
        }
    }

    public readonly record struct TimeRange(Seconds Min, Seconds Max)
    {
        public static TimeRange Radius(Seconds center, Seconds radius)
        {
            return new TimeRange(center - radius, center + radius);
        }
    }
}