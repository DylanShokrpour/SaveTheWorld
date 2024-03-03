using System.Threading.Tasks;
using Godot;
using GodotExtensions.extensions.node;

public partial class Spawner : Node3D {
	[Export] Path3D path3D;

	public override void _Ready() {
		GlobalPosition = path3D.Curve.GetPointPosition(0);
	}

	/// <returns>Once all the enemies in the wave have been spawned</returns>
	public async Task SpawnGoonsInWave(Wave wave) {
		foreach (PackedScene enemy in wave.enemyList) {
			await this.WaitForSeconds(wave.timeBetweenSpawn);
			SpawnGoon(enemy);
		}
	}

	void SpawnGoon(PackedScene goonPrefab) {
		var goon = goonPrefab.Instantiate<Goon>();
		Path path = new(goon.speed);
		path3D.AddChild(path);
		goon.path = path;
		path.AddChild(goon);
	}
}