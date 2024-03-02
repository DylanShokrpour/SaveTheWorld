using Godot;

public partial class Necromancer : Node3D {
	[Export] PackedScene spawnPrefab;
	[Export] int spawnTime;
	Path pathCategory;
	Timer spawnTimer;

	public override void _Ready() {
		ConfigureTimer();
		spawnTimer.Timeout += Spawn;
		pathCategory = GetParent().GetParent<Path>();
	}

	void Spawn() {
		var unit = spawnPrefab.Instantiate<Goon>();
		Path path;
		path = new(unit.speed);
		pathCategory.GetParent().AddChild(path);
		path.Progress = pathCategory.Progress;
		path.AddChild(unit);
	}


	void ConfigureTimer() {
		spawnTimer = new();
		spawnTimer.WaitTime = spawnTime;
		AddChild(spawnTimer);
		spawnTimer.Start();
	}
}