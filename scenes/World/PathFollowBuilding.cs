using Godot;

public partial class PathFollowBuilding : Node3D {
	[Export] public float speed;
	public Path path;

	public override void _Ready() {
		// This will be accessed by the spawner
	}

	public void ChangePathSpeed(float newSpeed) {
		path.speed = speed;
	}
}