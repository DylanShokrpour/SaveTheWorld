using System;
using System.Collections.Generic;
using Godot;
using GodotExtensions.extensions.node;

public partial class Healer : Area3D {
	[Export] int HealTime = 1;
	[Export] int healAmount = 10;
	Type targetType;
	Timer healTimer;
	List<Node3D> targets;


	public override void _Ready() {
		targetType = typeof(Goon);
		ConfigureTimer();
	}

	void GetBodies() {
		targets = new();
		foreach (Node3D body in GetOverlappingBodies())
			if (body.GetType() == targetType)
				targets.Add(body);
	}

	void Heal() {
		GetBodies();
		foreach (Node3D target in targets) {
			var healthComponent = target.GetFirstChildWhichIsA<HealthComponent>();
			healthComponent.AddHealth(healAmount);
		}
	}

	void ConfigureTimer() {
		healTimer = new();
		healTimer.WaitTime = HealTime;
		AddChild(healTimer);
		healTimer.Start();
		healTimer.Timeout += Heal;
	}
}