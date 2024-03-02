using System;
using System.Collections.Generic;
using Godot;
using GodotExtensions.extensions.node;

public partial class AreaDamageComponent : Area3D {
	readonly List<Node3D> enemiesInRange = new();
	Type targetType;


	void GetBodies() {
		targetType = GetParent<Bullet>().targetType;
		foreach (Node3D body in GetOverlappingBodies())
			if (body.GetType() == targetType)
				enemiesInRange.Add(body);
	}

	public void Impact() {
		GetBodies();
		foreach (Node3D target in enemiesInRange) {
			var targetHealth = target.GetFirstChildOfClass<HealthComponent>();
			targetHealth.TakeDamage(GetParent<Bullet>().damage);
		}
	}
}