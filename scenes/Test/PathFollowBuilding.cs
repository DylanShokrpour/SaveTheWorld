using Godot;

public partial class PathFollowBuilding : Node3D {
	public Path path;
	[Export] public float speed;

	public override void _Ready() {
		// This will be accessed by the spawner
	}

	public void ChangePathSpeed(float newSpeed) {
		path.Speed = speed;
	}
}