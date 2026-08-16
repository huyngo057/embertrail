using Godot;
using PlayerController = Embertrail.Entities.Player.Player;

namespace Embertrail.Entities.Enemy;

public partial class Enemy : CharacterBody2D
{
	[Export] public float Speed { get; set; } = 70f;
	[Export] public NodePath PlayerPath { get; set; } = new("../Player");

	private PlayerController? _player;

	public override void _Ready()
	{
		_player = GetNodeOrNull<PlayerController>(PlayerPath);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_player is null || !GodotObject.IsInstanceValid(_player))
		{
			return;
		}

		Vector2 toPlayer = _player.GlobalPosition - GlobalPosition;
		if (toPlayer.LengthSquared() < 4f)
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();
			return;
		}

		Velocity = toPlayer.Normalized() * Speed;
		MoveAndSlide();
	}
}
