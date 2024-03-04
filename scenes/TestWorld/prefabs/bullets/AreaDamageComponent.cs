using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotExtensions.extensions.node;

public partial class AreaDamageComponent : Area3D {

	public void Impact(int damage, Type targetType) {
		foreach (Node3D target in GetEnemiesInRange(targetType)) {
			var targetHealth = target.FindFirstChildOfClass<HealthComponent>();
			targetHealth.TakeDamage(damage);
		}
	}

	List<Node3D> GetEnemiesInRange(Type targetType) {
		return GetOverlappingBodies()
			.Where(body => body.GetType() == targetType)
			.ToList();
	}
}