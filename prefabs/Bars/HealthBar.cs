using Godot;
using System;

public partial class HealthBar : ProgressBar {
	HealthComponent healthComponent;
	public override void _Ready() {
		healthComponent = GetNode<HealthComponent>("%HealthComponent");
		Value = healthComponent.health;
		healthComponent.OnHealthChange += updateBar;
	}

	public void updateBar() {
		Value = healthComponent.health;
	}
}
