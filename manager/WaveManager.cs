using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class WaveManager : Node3D {
	List<Wave> waves = new List<Wave>();
	List<Spawner> spawners = new List<Spawner>();
	Wave currentWave;
	int waveCount;
	public override void _Ready() {
		Spawner spawner = GetNode<Spawner>("%TestSpawner");
		spawners.Add(spawner);
		newWave();
	}

	public void addWave(Wave wave) {
		waves.Add(wave);
	}

	public void newWave() {
		foreach (Spawner spawner in
		spawners) {
			currentWave = waves[waveCount];
			spawner.startWave(currentWave);
			
		}
		waveCount++;
	}
}
