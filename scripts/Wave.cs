using Godot;
using Godot.Collections;

public partial class Wave : Node {
	[Signal]
	public delegate void OnWaveStartEventHandler();
	//public List<PackedScene> enemies = new List<PackedScene>();

	[Export] public Array<PackedScene> enemyList;
	[Export] public int timeBetweenSpawn;

	public void AddEnemy(PackedScene goon) {
		if (goon != null)
			enemyList.Add(goon);
	}

	public void StartWave() {
		EmitSignal(SignalName.OnWaveStart);
	}
}