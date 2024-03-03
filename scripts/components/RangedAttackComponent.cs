using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

/// <summary>
///     RangeComponent is a component that allows the parent to attack enemies in a certain range.
/// </summary>
public partial class RangedAttackComponent : Node3D {
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
		ConfigureTimer(); // MAKE A LIST THAT STORES THE CLOSEST GOONS
		detectionComponent = GetNode<DetectionComponent>("RangeDetectionComponent");
		detectionComponent.ValidBodyEntered += OnDetected;
		detectionComponent.ValidBodyExited += GetNextTarget;
		attackTimer.Timeout += Attack;
	}


	void OnLost() {
		enemiesInArea.Remove(currentTarget);
		RemoveInvalidGoons();
		hasTarget = false;
	}

	void OnDetected(CharacterBody3D target) {
		if (target.GetType() == targetType)
			enemiesInArea.Add(target);
		RemoveInvalidGoons();
		if (enemiesInArea.Count == 0)
			return;
		currentTarget = enemiesInArea[0];
	}

	void Attack() {
		RemoveInvalidGoons();
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
		
		RemoveInvalidGoons();
		if (enemiesInArea.Count == 0)
			return;
		currentTarget = enemiesInArea[0];
	}

	void RemoveInvalidGoons() {
		enemiesInArea.RemoveAll(goon => !IsInstanceValid(goon));
	}
	
	void PrintGoonList(List<Node3D> list) {
		GD.Print("_____________________________");
		foreach (Node3D goon in list)
			if (!IsInstanceValid(goon))
				GD.Print("NONVALID");
			else
				GD.Print("Goon: " + goon.Name);
	}
}