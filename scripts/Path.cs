using Godot;
using System;

public partial class Path : PathFollow3D {
	float Speed;

	public Path(float Speed) {
		this.Speed = Speed;
	}

	public override void _PhysicsProcess(double delta) {
	Progress += Speed * (float)delta;
	}
}
