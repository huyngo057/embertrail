using Embertrail.Components;
using Godot;
using PlayerController = Embertrail.Entities.Player.Player;

namespace Embertrail.Entities.Enemy;

public partial class Enemy : CharacterBody2D
{
	[Signal] public delegate void DiedEventHandler();

	[Export] public float Speed { get; set; } = 70f;
	[Export] public int ContactDamage { get; set; } = 1;
	[Export] public float ContactDamageInterval { get; set; } = 0.8f;
	[Export] public float ContactRange { get; set; } = 34f;

	private HealthComponent _health = null!;
	private PlayerController? _player;
	private float _damageCooldownLeft;

	public override void _Ready()
	{
		_health = GetNode<HealthComponent>("Health");
		_health.Died += OnDied;
		_player = GetTree().GetFirstNodeInGroup("player") as PlayerController;
	}

	public void ApplyDamage(int amount)
	{
		_health.TakeDamage(amount);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_damageCooldownLeft > 0f)
		{
			_damageCooldownLeft -= (float)delta;
		}

		if (_player is null || !IsInstanceValid(_player) || !_player.Health.IsAlive)
		{
			Velocity = Vector2.Zero;
			return;
		}

		Vector2 toPlayer = _player.GlobalPosition - GlobalPosition;
		float distance = toPlayer.Length();

		Velocity = distance < 2f ? Vector2.Zero : toPlayer / distance * Speed;
		MoveAndSlide();

		if (_damageCooldownLeft <= 0f && distance <= ContactRange)
		{
			_player.ApplyDamage(ContactDamage);
			_damageCooldownLeft = ContactDamageInterval;
		}
	}

	private void OnDied()
	{
		EmitSignal(SignalName.Died);
		QueueFree();
	}
}
