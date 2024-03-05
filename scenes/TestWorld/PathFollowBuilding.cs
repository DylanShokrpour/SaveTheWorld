using Godot;

public partial class PathFollowBuilding : Node3D {
	[Export] public float speed;
	public AutoPathFollow autoPathFollow;

	public override void _Ready() {
		// This will be accessed by the spawner
	}

	public void ChangePathSpeed(float newSpeed) {
		autoPathFollow.speed = speed;
	}
}