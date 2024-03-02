using System;
using System.Collections.Generic;
using Godot;
using GodotExtensions.extensions.node;

public partial class AreaDamageComponent : Area3D {
	Type targetType;
	readonly List<Node3D> enemiesInRange = new();


	void GetBodies() {
		targetType = GetParent<Bullet>().targetType;
		foreach (Node3D body in GetOverlappingBodies())
			if (body.GetType() == targetType)
				enemiesInRange.Add(body);
	}

	public void impact() {
		GetBodies();
		foreach (Node3D target in enemiesInRange) {
			HealthComponent targetHealth = target.GetFirstChildOfClass<HealthComponent>();
			targetHealth.TakeDamage(GetParent<Bullet>().damage);
		}
	}
}