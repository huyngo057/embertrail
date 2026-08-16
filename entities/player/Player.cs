using Godot;

namespace Embertrail.Entities.Player;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed { get; set; } = 220f;
	[Export] public float DashSpeed { get; set; } = 480f;
	[Export] public float DashDuration { get; set; } = 0.12f;

	private float _dashTimeLeft;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 input = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		if (Input.IsActionJustPressed("dash") && _dashTimeLeft <= 0f && input != Vector2.Zero)
		{
			_dashTimeLeft = DashDuration;
		}

		float speed = _dashTimeLeft > 0f ? DashSpeed : Speed;
		if (_dashTimeLeft > 0f)
		{
			_dashTimeLeft -= (float)delta;
		}

		Velocity = input == Vector2.Zero ? Vector2.Zero : input.Normalized() * speed;
		MoveAndSlide();
	}
}
