using Godot;
using GodotExtensions.extensions.node;

public partial class Factory : Node3D {
	[Export] PackedScene spawnPrefab;
	[Export] int spawnTime;
	Timer spawnTimer;
	Path3D pathFollowed;

	public override void _Ready() {
		ConfigureTimer();
		spawnTimer.Timeout += Spawn;
		pathFollowed = GetParent<Base>().path;
	}

	void Spawn() {
		// ! Does not queue free when reaches end of the path
		var unit = spawnPrefab.Instantiate<Node3D>();
		var trackBuilding = unit.FindFirstChild<PathFollowBuilding>();
		AutoPathFollow autoPathFollow = new(-trackBuilding.speed);
		pathFollowed.AddChild(autoPathFollow);
		trackBuilding.autoPathFollow = autoPathFollow;
		autoPathFollow.ProgressRatio = 1f;
		autoPathFollow.AddChild(unit);
	}

	void ChangePathFollowed(Path3D newPath) {
		pathFollowed = newPath;
	}


	void ConfigureTimer() {
		spawnTimer = new();
		spawnTimer.WaitTime = spawnTime;
		AddChild(spawnTimer);
		spawnTimer.Start();
	}
}