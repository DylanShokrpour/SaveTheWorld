using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotExtensions.extensions.node;

/// <summary>
///     This class is responsible for managing the waves of enemies
/// </summary>
public partial class WaveManager : Node3D {
	[Signal] public delegate void NewWaveStartedEventHandler();

	[Export] int waveDuration;
	Timer waveTimer = new();
	List<Wave> waves = new();
	List<Spawner> spawners = new();
	Wave currentWave;
	public int waveCount;
	public int totalWaves;

	public override void _Ready() {
		ConfigureTimer();
		waves = this.GetDescendants<Wave>().ToList();
		totalWaves = waves.Count;
		spawners = this.GetDescendants<Spawner>().ToList();
		waveTimer.Timeout += StartNewWave;
		StartNewWave();
	}
	
	void ConfigureTimer() {
		waveTimer = new();
		waveTimer.WaitTime = waveDuration;
		AddChild(waveTimer);
		waveTimer.Start();
	}

	void StartNewWave() {
		GD.Print("Starting new wave");
		foreach (Spawner spawner in spawners) {
			// ! An exception will be thrown when the waveCount exceeds the number of waves
			currentWave = waves[waveCount];
			_ = spawner.SpawnGoonsInWave(currentWave);
		}

		waveCount++;
		EmitSignal(SignalName.NewWaveStarted);
	}
}