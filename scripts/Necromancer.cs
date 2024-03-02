using Godot;

public partial class Necromancer : Node3D {
	[Export] PackedScene spawnPrefab;
	[Export] int spawnTime;
	Path pathCategory;
	Timer spawnTimer;

	public override void _Ready() {
		ConfigureTimer();
		spawnTimer.Timeout += spawn;
		pathCategory = GetParent().GetParent<Path>();
	}

	public void spawn() {
		Goon unit = spawnPrefab.Instantiate<Goon>();
		Path path;
		path = new Path(unit.speed);
		pathCategory.GetParent().AddChild(path);
		path.Progress = pathCategory.Progress;
		path.AddChild(unit);
	}


	void ConfigureTimer() {
		spawnTimer = new Timer();
		spawnTimer.WaitTime = spawnTime;
		AddChild(spawnTimer);
		spawnTimer.Start();
	}
}