using Godot;

public partial class World : Node3D {
	// lol the export doesn't even work but Hoy Hoy HOy
	[Export] Wave wave1;
	[Export] Wave wave2;

	LocationLabel locationLabel;

	Spawner spawner2;
	WaveManager waveManager;
	AudioManager audioManager;


	public override void _Ready() {
		spawner2 = GetNode<Spawner>("%TestSpawner2");

		audioManager = GetNode<AudioManager>("AudioManager");
		audioManager.PlayRandomAmbiance();

		locationLabel = GetNode<LocationLabel>("%LocationLabel");
		locationLabel.Text = Name;
		locationLabel.Activate();

		waveManager = GetNode<WaveManager>("WaveManager");
		waveManager.NewWaveStarted += CheckForMusicChange;
	}

	void CheckForMusicChange() {
		if ((waveManager.totalWaves / 2) == waveManager.waveCount)
			audioManager.PlayRandomEpicAmbiance();
	}
}