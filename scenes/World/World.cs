using Godot;

public partial class World : Node3D {
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
		_ = locationLabel.ShowLocationAnimation();

		waveManager = GetNode<WaveManager>("WaveManager");
		waveManager.NewWaveStarted += CheckForMusicChange;
	}

	void CheckForMusicChange() {
		if ((waveManager.totalWaves / 2) == waveManager.waveCount)
			audioManager.PlayRandomEpicAmbiance();
	}
}