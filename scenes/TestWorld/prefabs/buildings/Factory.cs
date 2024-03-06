using Godot;
using GodotExtensions.extensions.node;
using GodotExtensions.scripts.onready;

public partial class Factory : Node3D {
	[Export] PackedScene spawnPrefab;
	[Export] int spawnTime;
	[OnReady("Spawner")] Spawner spawner;
	Timer spawnTimer;

	public override void _Ready() {
		this.InitializeOnReadyVariables();
		// not ideal but idc.
		spawner.path = GetParent<Base>().path;
		spawner.spawnNode = GetTree().CurrentScene.GetNode("Buildings");

		ConfigureTimer();
		spawnTimer.Timeout += Spawn; // ! Only Authority should spawn...
	}

	void Spawn() {
		var instance = spawnPrefab.Instantiate<Node3D>();
		var trackBuilding = instance.FindFirstChild<PathFollowBuilding>();
		var pathFollow = new AutoPathFollow(instance.Name, trackBuilding.speed, AutoPathFollow.Direction.Backward);
		pathFollow.PathWrapping += () => { instance.QueueFree(); };

		spawner.SpawnUnit(instance, pathFollow);
	}

	void ConfigureTimer() {
		spawnTimer = new();
		spawnTimer.WaitTime = spawnTime;
		AddChild(spawnTimer);
		spawnTimer.Start();
	}
}