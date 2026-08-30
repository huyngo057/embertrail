using Godot;
using PlayerController = Embertrail.Entities.Player.Player;

namespace Embertrail.UI;

/// Display only: reads state through signals, never changes gameplay values.
public partial class Hud : CanvasLayer
{
	private Label _hpLabel = null!;
	private Label _messageLabel = null!;

	public override void _Ready()
	{
		_hpLabel = GetNode<Label>("HpLabel");
		_messageLabel = GetNode<Label>("MessageLabel");
		_messageLabel.Text = string.Empty;
	}

	public void BindPlayer(PlayerController player)
	{
		player.Health.HealthChanged += OnHealthChanged;
		OnHealthChanged(player.Health.Current, player.Health.MaxHealth);
	}

	public void ShowMessage(string text)
	{
		_messageLabel.Text = text;
	}

	private void OnHealthChanged(int current, int max)
	{
		_hpLabel.Text = $"HP {current}/{max}";
	}
}
