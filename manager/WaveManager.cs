using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotExtensions.extensions.node;

/// <summary>
///     This class is responsible for managing the waves of enemies
/// </summary>
public partial class WaveManager : Node3D {
	[Signal]
	public delegate void newWaveStartEventHandler();

	[Export] int waveSpeed;
	Timer waveTimer = new();
	List<Wave> waves = new();
	List<Spawner> spawners = new();
	Wave currentWave;
	public int waveCount;
	public int totalWaves;

	public override void _Ready() {
		ConfigureTimer();
		waves = this.GetDescendantsWhichAreA<Wave>().ToList();
		totalWaves = waves.Count;
		spawners = this.GetDescendantsWhichAreA<Spawner>().ToList();
		//Spawner spawner = GetNode<Spawner>("%TestSpawner");
		//spawners.Add(spawner);
		waveTimer.Timeout += newWave;
		newWave();
	}


	public void addSpawner(Spawner spawn) {
		spawners.Add(spawn);
	}

	public void newWave() {
		foreach (Spawner spawner in
		         spawners) {
			currentWave = waves[waveCount];
			spawner.startWave(currentWave);
		}

		waveCount++;
		EmitSignal(SignalName.newWaveStart);
	}

	void ConfigureTimer() {
		waveTimer = new Timer();
		waveTimer.WaitTime = waveSpeed;
		AddChild(waveTimer);
		waveTimer.Start();
	}
}