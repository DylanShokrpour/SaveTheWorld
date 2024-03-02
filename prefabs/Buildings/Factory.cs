using Godot;
using GodotExtensions.extensions.node;

public partial class Factory : Node3D {
	[Export] PackedScene spawnPrefab;
	[Export] int spawnTime;
	Timer spawnTimer;
	Path3D pathFollowed;

	public override void _Ready() {
		ConfigureTimer();
		spawnTimer.Timeout += spawn;
		pathFollowed = GetParent<Base>().path;
	}

	public void spawn() {
		Node3D unit = spawnPrefab.Instantiate<Node3D>();
		Path path;
		PathFollowBuilding trackBuilding = unit.GetFirstChildWhichIsA<PathFollowBuilding>();
		path = new Path(-trackBuilding.speed);
		pathFollowed.AddChild(path);
		trackBuilding.path = path;
		path.ProgressRatio = 1f;
		path.AddChild(unit);
	}

	void changePathFollowed(Path3D newPath) {
		pathFollowed = newPath;
	}


	void ConfigureTimer() {
		spawnTimer = new Timer();
		spawnTimer.WaitTime = spawnTime;
		AddChild(spawnTimer);
		spawnTimer.Start();
	}
}