using Godot;

public partial class AutoPathFollow : PathFollow3D {
	public float speed;

	public AutoPathFollow(float speed) {
		this.speed = speed;
	}

	public override void _PhysicsProcess(double delta) {
		Progress += speed * (float)delta;
	}
}