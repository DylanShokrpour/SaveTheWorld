using Godot;
using GodotExtensions.extensions.node;

public partial class Necromancer : Node3D {
	[Export] PackedScene spawnPrefab;
	[Export] int spawnTime;
	AutoPathFollow autoPathFollowCategory;
	Timer spawnTimer;
	Spawner spawner; // TODO: get a reference to a spawner....

	public override void _Ready() {
		ConfigureTimer();
		spawnTimer.Timeout += Spawn; // ! Only Authority should spawn...
		autoPathFollowCategory = this.FindFirstAncestorOrNull<AutoPathFollow>(); // Will only exist on server
	}

	void Spawn() {
		var unit = spawnPrefab.Instantiate<Goon>();
		AutoPathFollow autoPathFollow = new(unit.Name, unit.speed, customProgress: autoPathFollowCategory.Progress);
		spawner.SpawnUnit(unit, autoPathFollow);
	}

	void ConfigureTimer() {
		spawnTimer = new();
		spawnTimer.WaitTime = spawnTime;
		AddChild(spawnTimer);
		spawnTimer.Start();
	}
}