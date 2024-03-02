using Godot;

public partial class Spawner : Node3D {

	//[Export] PathFollow3D path;
	[Export] Path3D path3D;
	[Export] int timeBetweenSpawns = 5;
	[Export] Timer spawnTimer;
	public Wave currentWave;

	public override void _Ready() {
		GlobalPosition = path3D.Curve.GetPointPosition(0);
	}

	public async void StartWave(Wave wave) {
		foreach (PackedScene enemy in wave.enemyList) {
			await ToSignal(GetTree().CreateTimer(wave.timeBetweenSpawn), SceneTreeTimer.SignalName.Timeout);
			VanguardFactory(enemy);
		}
	}

	void VanguardFactory(PackedScene goonPrefab) {
		//Maybe it should have its own script but I will cry later when that bites my in the a

		var goon = goonPrefab.Instantiate<Goon>();
		Path path = new(goon.speed);
		path3D.AddChild(path);
		goon.path = path;
		path.AddChild(goon);
		//Bullet bulletInstance = bullet.Instantiate<Bullet>();
	}
}