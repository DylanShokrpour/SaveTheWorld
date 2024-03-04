using Godot;

public partial class Necromancer : Node3D {
	[Export] PackedScene spawnPrefab;
	[Export] int spawnTime;
	AutoPathFollow autoPathFollowCategory;
	Timer spawnTimer;

	public override void _Ready() {
		ConfigureTimer();
		spawnTimer.Timeout += Spawn;
		autoPathFollowCategory = GetParent().GetParent<AutoPathFollow>();
	}

	void Spawn() {
		var unit = spawnPrefab.Instantiate<Goon>();
		AutoPathFollow autoPathFollow = new(unit.speed);
		autoPathFollowCategory.GetParent().AddChild(autoPathFollow);
		autoPathFollow.Progress = autoPathFollowCategory.Progress;
		autoPathFollow.AddChild(unit);
	}

	void ConfigureTimer() {
		spawnTimer = new();
		spawnTimer.WaitTime = spawnTime;
		AddChild(spawnTimer);
		spawnTimer.Start();
	}
}