using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
			Wave currentWave = waves[waveCount];
			_ = SpawnGoonsInWave(spawner, currentWave);
		}

		waveCount++;
		EmitSignal(SignalName.NewWaveStarted);
	}

	/// <returns>Once all the enemies in the wave have been spawned</returns>
	async Task SpawnGoonsInWave(Spawner spawner, Wave wave) {
		foreach (PackedScene enemyPrefab in wave.enemyList) {
			await this.WaitForSeconds(wave.timeBetweenSpawn);
			var enemy = enemyPrefab.Instantiate<Goon>();
			var autoPathFollow = new AutoPathFollow(enemy.Name, enemy.speed);
			enemy.autoPathFollow = autoPathFollow; // ! Not replicated
			spawner.SpawnUnit(enemy, autoPathFollow);
		}
	}
}