using Godot;

namespace Embertrail.Components;

/// Sole owner of hit points. Other nodes request changes and react to signals;
/// nobody writes Current from the outside.
public partial class HealthComponent : Node
{
	[Signal] public delegate void HealthChangedEventHandler(int current, int max);

	[Signal] public delegate void DiedEventHandler();

	[Export] public int MaxHealth { get; set; } = 5;

	public int Current { get; private set; }

	public bool IsAlive => Current > 0;

	// Runs before every _Ready in the tree, so listeners can read Current safely.
	public override void _EnterTree()
	{
		Current = MaxHealth;
	}

	public void TakeDamage(int amount)
	{
		if (amount <= 0 || !IsAlive)
		{
			return;
		}

		Current = Mathf.Max(Current - amount, 0);
		EmitSignal(SignalName.HealthChanged, Current, MaxHealth);

		if (!IsAlive)
		{
			EmitSignal(SignalName.Died);
		}
	}
}
