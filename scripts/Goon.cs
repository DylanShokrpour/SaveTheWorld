using System.Collections.Generic;
using Godot;

public partial class Goon : CharacterBody3D {
	[Export] public int damage = 10;
	[Export] public float speed = 1;
	public HealthComponent HealthComponent { get; private set; }
	public Path path;
	public List<Item> items = new();

	public override void _EnterTree() {
		HealthComponent = GetNode<HealthComponent>("%HealthComponent");
		HealthComponent.OnDeath += Die;
	}

	void Die() {
		GetParent().QueueFree();
	}
}