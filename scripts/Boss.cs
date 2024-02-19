using Godot;
using System;

public partial class Boss : Node3D
{
	[Export] int health;
	[Export] int damage;
	[Export] AudioStreamPlayer3D bossMusic;
	HealthComponent healthComponent;
	public override void _Ready()
	{
		healthComponent = GetNode<HealthComponent>("HealthComponent");
		healthComponent.SetHealth(health);
		bossMusic.Play();
	}
}
