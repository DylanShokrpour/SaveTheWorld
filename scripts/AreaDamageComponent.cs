using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotExtensions.extensions.node;

public partial class AreaDamageComponent : Area3D {
	Type targetType;

	public void Impact(int damage) {
		foreach (Node3D target in GetEnemiesInRange()) {
			var targetHealth = target.FindFirstChildOfClass<HealthComponent>();
			targetHealth.TakeDamage(damage);
		}
	}

	List<Node3D> GetEnemiesInRange() {
		targetType = GetParent<Bullet>().targetType;

		return GetOverlappingBodies()
			.Where(body => body.GetType() == targetType)
			.ToList();
	}
}