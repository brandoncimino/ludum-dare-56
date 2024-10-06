using Godot;
using System;
using System.Linq;
using ludumdare56;

public partial class BacteriumArea : Area3D
{
	private const int InitialHp = 100;
	private int _currentHp = InitialHp;
	private int _currentScore;

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

	public void Damage(int amount)
	{
		_currentHp -= amount;
		if (_currentHp <= 0)
		{
			Die();
		}
	}

	public void Score(int points)
	{
		_currentScore += points;
	}

	public void Die()
	{
		Console.WriteLine("you got dead again");
	}
}
