using System.Collections.Generic;
using Godot;

public partial class Goon : CharacterBody3D {
	[Export] public int damage = 10;
	[Export] public float speed = 1;
	public HealthComponent healthComponent;
	public Path path;
	public PathFollow3D parentFollow;
	public List<Item> items = new();

	public override void _EnterTree() {
		healthComponent = GetNode<HealthComponent>("%HealthComponent");
		healthComponent.OnDeath += die;
	}

	public void die() {
		GetParent().QueueFree();
	}
}