using System;
using Godot;
using GodotExtensions.extensions.node;

public partial class BlockComponent : DetectionComponent {
	HealthComponent healthComponentOfParent;
	public Type targetType = typeof(Goon);

	public override void _Ready() {
		base._Ready();
		ValidBodyEntered += block;
		healthComponentOfParent = GetParent().GetFirstChildWhichIsA<HealthComponent>();
	}

	public void block(CharacterBody3D body) {
		if (body.GetType() == targetType) {
			HealthComponent opponentHealthComponent = body.GetFirstChildWhichIsA<HealthComponent>();
			healthComponentOfParent.TakeDamage(opponentHealthComponent.GetHealth());
			opponentHealthComponent.TakeDamage(healthComponentOfParent.GetHealth());
		}

		if (healthComponentOfParent.GetHealth() <= 0) GetParent().QueueFree();
	}
	//Game.GetService<Players>().PlayerJoined += player => {
	//if (player.DisplayName.Contains("ROYAN") {
	//	player.Kick("Sorry, there was a problem loading your data. Please try again later");
	//}
	//} 
}