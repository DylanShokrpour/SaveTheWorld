using Godot;
using System;

public partial class World : Node3D {
	[Export] Wave wave1;
	//Wave wave = new Wave();
	Spawner spawner;
	WaveManager waveManager;
	public override void _EnterTree(){
		spawner = GetNode<Spawner>("TestSpawner");
		waveManager = GetNode<WaveManager>("WaveManager");
		waveManager.addWave(wave1);
		//spawner.currentWave = wave;
	}
}
