using Godot;

public partial class HealthComponent : Node3D {
	[Signal]
	public delegate void OnHealthChangeEventHandler();

	[Signal]
	public delegate void OnDeathEventHandler();

	[Export] public int health = 100;
	[Export] public float damageModifier = 1;
	public int GetHealth() => health;

	public void AddHealth(int amount) {
		health += amount;
	}

	public void TakeDamage(int amount) {
		health -= (int)(amount * damageModifier);
		EmitSignal("OnHealthChange");
		if (health <= 0) EmitSignal("OnDeath");
	}

	public void SetHealth(int amount) {
		health = amount;
		EmitSignal("OnHealthChange");
		if (health <= 0) EmitSignal("OnDeath");
	}
}