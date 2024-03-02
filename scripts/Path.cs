using Godot;

public partial class Path : PathFollow3D {
	public float Speed;

	public Path(float Speed) {
		this.Speed = Speed;
	}

	public override void _PhysicsProcess(double delta) {
		Progress += Speed * (float)delta;
	}
}