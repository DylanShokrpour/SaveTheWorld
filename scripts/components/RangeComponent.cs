using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

/// <summary>
///     RangeComponent is a component that allows the parent to attack enemies in a certain range.
/// </summary>
public partial class RangeComponent : Node3D {
	[Export] int damage; // this is the damage the user does I KNOW I AM HILAROUS FOR USING TROLL COMMENTS
	[Export] PackedScene prefabBullet;
	[Export] int AttackSpeed = 1; //time per shot so yeah kinda the opposite...
	List<Node3D> enemiesInArea = new();
	public Type targetType;
	Timer attackTimer;
	DetectionComponent detectionComponent;
	Node3D currentTarget;
	bool hasTarget;

	public override void _Ready() {
		FindTargetType();
		ConfigureTimer(); // MAKE A LIST THAT STORES THE CLOSEST GOONS
		detectionComponent = GetNode<DetectionComponent>("RangeDetectionComponent");
		detectionComponent.ValidBodyEntered += OnDetected;
		detectionComponent.ValidBodyExited += GetNextTarget;
		attackTimer.Timeout += Attack;
	}

	void FindTargetType() {
		if ((GetParent().GetType() == typeof(Building)) || (GetParent().GetType() == typeof(Base)))
			targetType = typeof(Goon);
		else
			targetType = typeof(Character);
	}

	void OnLost() {
		enemiesInArea.Remove(currentTarget);
		enemiesInArea = GarbageCollector();
		hasTarget = false;
		//attackTimer.Stop();
	}

	void OnDetected(CharacterBody3D target) {
		if (target.GetType() == targetType)
			enemiesInArea.Add(target);
		enemiesInArea = GarbageCollector();
		if (enemiesInArea.Count == 0)
			return;
		currentTarget = enemiesInArea[0];
	}

	void Attack() {
		enemiesInArea = GarbageCollector();
		if (enemiesInArea.Count == 0)
			return;
		if (!IsInstanceValid(enemiesInArea[0]))
			return;
		if (!IsInstanceValid(currentTarget))
			return;
		if (currentTarget.GetParent() is Path path)
			ShootBulletAtTarget(path);
		else
			ShootBulletAtTarget(currentTarget);
	}

	void ShootBulletAtTarget(Node3D targetLocation) {
		Bullet bullet = CreateBullet();
		bullet.targetType = targetType;
		bullet.targetPosition = targetLocation.GlobalPosition;
		bullet.damage = damage;
		bullet.OnCreated(targetType);
	}

	Bullet CreateBullet() {
		var bullet = prefabBullet.Instantiate<Bullet>();
		GetTree().CurrentScene.GetNode("%Bullets").AddChild(bullet);
		bullet.GlobalPosition = GetParent<Node3D>().GlobalPosition;
		return bullet;
	}

	void ConfigureTimer() {
		attackTimer = new();
		attackTimer.WaitTime = AttackSpeed;
		AddChild(attackTimer);
		attackTimer.Start();
	}

	void GetNextTarget() {
		OnLost();
		enemiesInArea = GarbageCollector();
		if (enemiesInArea.Count == 0)
			return;
		currentTarget = enemiesInArea[0];
	}

	void PrintGoonList(List<Node3D> list) {
		GD.Print("_____________________________");
		foreach (Node3D goon in list)
			if (!IsInstanceValid(goon))
				GD.Print("NONVALID");
			else
				GD.Print(goon);
	}


	List<Node3D> GarbageCollector() {
		List<Node3D> newList = enemiesInArea.Where(goon => (goon != null) && IsInstanceValid(goon)).ToList();
		PrintGoonList(newList);
		return newList;
	}
}