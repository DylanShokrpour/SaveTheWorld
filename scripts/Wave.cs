using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Wave : Node {
	[Signal]
	public delegate void OnWaveStartEventHandler();
	//public List<PackedScene> enemies = new List<PackedScene>();
	
	[Export]
	public Godot.Collections.Array<PackedScene> enemyList;
	[Export] public int timeBetweenSpawn;
	public void addEnemy(PackedScene goon) {
		if (goon != null) {
			enemyList.Add(goon);
		}
	}

	public void StartWave() {
		EmitSignal(SignalName.OnWaveStart);
	}
}
