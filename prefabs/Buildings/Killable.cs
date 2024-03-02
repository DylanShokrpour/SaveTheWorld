using Godot;

public partial class Killable : Node3D {
	HealthComponent healthComponent;

	public override void _Ready() {
		healthComponent = GetNode<HealthComponent>("%HealthComponent");
		healthComponent.OnDeath += die;
	}

	void die() {
		QueueFree();
	}
}