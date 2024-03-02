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

	public void Spawn() {
		var unit = spawnPrefab.Instantiate<Node3D>();
		Path path;
		var trackBuilding = unit.GetFirstChildWhichIsA<PathFollowBuilding>();
		path = new(-trackBuilding.speed);
		pathFollowed.AddChild(path);
		trackBuilding.path = path;
		path.ProgressRatio = 1f;
		path.AddChild(unit);
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