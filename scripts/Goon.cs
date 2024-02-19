using Godot;
using System;

public partial class Goon : CharacterBody3D {
	[Export] public int damage = 10;
	[Export] public int speed = 1;
	public HealthComponent healthComponent;
	public PathFollow3D path;
	public override void _Ready() {
		healthComponent = GetNode<HealthComponent>("HealthComponent");
		
	}
}
