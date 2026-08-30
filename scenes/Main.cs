using Embertrail.UI;
using Godot;
using EnemyUnit = Embertrail.Entities.Enemy.Enemy;
using PlayerController = Embertrail.Entities.Player.Player;

namespace Embertrail.Scenes;

/// Round referee: watches the entities, tells the HUD what to show, restarts.
public partial class Main : Node2D
{
	private Hud _hud = null!;
	private PlayerController _player = null!;
	private int _enemiesLeft;
	private bool _roundOver;

	public override void _Ready()
	{
		_hud = GetNode<Hud>("Hud");
		_player = GetNode<PlayerController>("Player");

		_hud.BindPlayer(_player);
		_player.Died += OnPlayerDied;

		foreach (Node node in GetNode("Enemies").GetChildren())
		{
			if (node is not EnemyUnit enemy)
			{
				continue;
			}

			_enemiesLeft++;
			enemy.Died += OnEnemyDied;
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (_roundOver && @event.IsActionPressed("restart"))
		{
			GetTree().ReloadCurrentScene();
		}
	}

	private void OnEnemyDied()
	{
		_enemiesLeft--;
		if (_enemiesLeft <= 0)
		{
			EndRound("Region cleared. Press R to run it again.");
		}
	}

	private void OnPlayerDied()
	{
		EndRound("The trail claims you. Press R to retry.");
	}

	private void EndRound(string message)
	{
		if (_roundOver)
		{
			return;
		}

		_roundOver = true;
		_hud.ShowMessage(message);
	}
}
