using System;
using System.Collections.Generic;
using Godot;
using GodotExtensions.extensions.node;

public partial class Healer : Area3D {
	Type targetType;
	Timer healTimer;
	[Export] int HealTime = 1;
	[Export] int healAmount = 10;
	List<Node3D> targets;


	public override void _Ready() {
		targetType = typeof(Goon);
		ConfigureTimer();
	}

	void GetBodies() {
		targets = new List<Node3D>();
		foreach (Node3D body in GetOverlappingBodies())
			if (body.GetType() == targetType)
				targets.Add(body);
	}

	void heal() {
		GetBodies();
		foreach (Node3D target in targets) {
			HealthComponent healthComponent = target.GetFirstChildWhichIsA<HealthComponent>();
			healthComponent.AddHealth(healAmount);
		}
	}

	void ConfigureTimer() {
		healTimer = new Timer();
		healTimer.WaitTime = HealTime;
		AddChild(healTimer);
		healTimer.Start();
		healTimer.Timeout += heal;
	}
}