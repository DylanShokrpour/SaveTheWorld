using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotExtensions.extensions.node;

/// <summary>
///     This class is responsible for managing the waves of enemies
/// </summary>
public partial class WaveManager : Node3D {
	[Signal] public delegate void NewWaveStartedEventHandler();

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
		waveTimer.Timeout += NewWave;
		NewWave();
	}


	public void AddSpawner(Spawner spawn) {
		spawners.Add(spawn);
	}

	void NewWave() {
		foreach (Spawner spawner in
		         spawners) {
			currentWave = waves[waveCount];
			spawner.StartWave(currentWave);
		}

		waveCount++;
		EmitSignal(SignalName.NewWaveStarted);
	}

	void ConfigureTimer() {
		waveTimer = new();
		waveTimer.WaitTime = waveSpeed;
		AddChild(waveTimer);
		waveTimer.Start();
	}
}