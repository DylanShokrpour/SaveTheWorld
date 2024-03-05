using Godot;
using GodotExtensions.extensions.node;

public partial class HealthBar : ProgressBar {
	HealthComponent healthComponent;

	public override void _Ready() {
		healthComponent = GetOwner<Node3D>().FindFirstChildOfClass<HealthComponent>();
		UpdateBar();
		healthComponent.OnHealthChanged += UpdateBar;
	}

	void UpdateBar() {
		Value = healthComponent.health;
	}
}