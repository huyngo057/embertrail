using Embertrail.Components;
using Godot;
using EnemyUnit = Embertrail.Entities.Enemy.Enemy;

namespace Embertrail.Entities.Player;

public partial class Player : CharacterBody2D
{
	[Signal] public delegate void DiedEventHandler();

	[Export] public float Speed { get; set; } = 220f;
	[Export] public float DashSpeed { get; set; } = 480f;
	[Export] public float DashDuration { get; set; } = 0.12f;
	[Export] public int AttackDamage { get; set; } = 2;
	[Export] public float AttackRange { get; set; } = 48f;
	[Export] public float AttackCooldown { get; set; } = 0.3f;
	[Export] public float InvulnerabilityTime { get; set; } = 0.7f;

	private static readonly Color NormalColor = new(0.95f, 0.55f, 0.2f);
	private static readonly Color HurtColor = new(1f, 1f, 1f);

	private HealthComponent _health = null!;
	private Polygon2D _body = null!;
	private Polygon2D _swing = null!;

	private Vector2 _facing = Vector2.Right;
	private float _dashTimeLeft;
	private float _attackCooldownLeft;
	private float _swingTimeLeft;
	private float _invulnerableTimeLeft;

	public HealthComponent Health => _health;

	public override void _Ready()
	{
		_health = GetNode<HealthComponent>("Health");
		_body = GetNode<Polygon2D>("Body");
		_swing = GetNode<Polygon2D>("Swing");

		_swing.Visible = false;
		_health.Died += OnDied;
	}

	/// Entry point for anything that wants to hurt the player.
	public void ApplyDamage(int amount)
	{
		if (_invulnerableTimeLeft > 0f || !_health.IsAlive)
		{
			return;
		}

		_invulnerableTimeLeft = InvulnerabilityTime;
		_health.TakeDamage(amount);
	}

	public override void _PhysicsProcess(double delta)
	{
		float dt = (float)delta;
		TickTimers(dt);

		if (!_health.IsAlive)
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();
			return;
		}

		Vector2 input = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (input != Vector2.Zero)
		{
			_facing = input.Normalized();
		}

		if (Input.IsActionJustPressed("dash") && _dashTimeLeft <= 0f && input != Vector2.Zero)
		{
			_dashTimeLeft = DashDuration;
		}

		if (Input.IsActionJustPressed("attack") && _attackCooldownLeft <= 0f)
		{
			Attack();
		}

		float speed = _dashTimeLeft > 0f ? DashSpeed : Speed;
		Velocity = input == Vector2.Zero ? Vector2.Zero : input.Normalized() * speed;
		MoveAndSlide();

		_swing.Position = _facing * 26f;
		_body.Color = _invulnerableTimeLeft > 0f ? HurtColor : NormalColor;
	}

	private void Attack()
	{
		_attackCooldownLeft = AttackCooldown;
		_swingTimeLeft = 0.12f;
		_swing.Visible = true;

		foreach (Node node in GetTree().GetNodesInGroup("enemy"))
		{
			if (node is not EnemyUnit enemy)
			{
				continue;
			}

			Vector2 toEnemy = enemy.GlobalPosition - GlobalPosition;
			bool inRange = toEnemy.Length() <= AttackRange;
			bool inFront = _facing.Dot(toEnemy.Normalized()) >= 0.25f;

			if (inRange && inFront)
			{
				enemy.ApplyDamage(AttackDamage);
			}
		}
	}

	private void TickTimers(float dt)
	{
		if (_dashTimeLeft > 0f)
		{
			_dashTimeLeft -= dt;
		}

		if (_attackCooldownLeft > 0f)
		{
			_attackCooldownLeft -= dt;
		}

		if (_invulnerableTimeLeft > 0f)
		{
			_invulnerableTimeLeft -= dt;
		}

		if (_swingTimeLeft > 0f)
		{
			_swingTimeLeft -= dt;
			if (_swingTimeLeft <= 0f)
			{
				_swing.Visible = false;
			}
		}
	}

	private void OnDied()
	{
		_body.Color = new Color(0.35f, 0.2f, 0.15f);
		_swing.Visible = false;
		EmitSignal(SignalName.Died);
	}
}
