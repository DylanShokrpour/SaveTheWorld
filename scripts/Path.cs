using Godot;

public partial class Path : PathFollow3D {
	public float speed;

	public Path(float speed) {
		this.speed = speed;
	}

	public override void _PhysicsProcess(double delta) {
		Progress += speed * (float)delta;
	}
}