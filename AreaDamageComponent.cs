using System;
using System.Collections.Generic;
using Godot;
using GodotExtensions.extensions.node;

public partial class AreaDamageComponent : Area3D {
	Type targetType;
	readonly List<Node3D> enemiesInRange = new();


	void GetBodies() {
		targetType = GetParent<Bullet>().targetType;
		foreach (Node3D body in GetOverlappingBodies()) {
			GD.Print(body.GetType() + " type " + targetType);
			if (body.GetType() == targetType)
				enemiesInRange.Add(body);
		}
	}

	public void impact() {
		GD.Print("IMPCAT");
		GetBodies();
		foreach (Node3D target in enemiesInRange) {
			HealthComponent targetHealth = target.GetFirstChildOfClass<HealthComponent>();
			GD.Print("AMOUNT " + targetHealth.Name);
			targetHealth.TakeDamage(GetParent<Bullet>().damage);
		}
	}
}