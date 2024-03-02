using Godot;
using GodotExtensions.extensions.node;

public partial class HealthBar : ProgressBar {
	HealthComponent healthComponent;

	public override void _Ready() {
		healthComponent = GetOwner<Node3D>().GetFirstChildOfClass<HealthComponent>();
		Value = healthComponent.health;
		healthComponent.OnHealthChange += UpdateBar;
	}

	public void UpdateBar() {
		Value = healthComponent.health;
	}
}