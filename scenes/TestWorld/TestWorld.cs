using Godot;

public partial class TestWorld : Node3D {
	WaveManager waveManager;
	AudioManager audioManager;


	public override void _Ready() {
		var spawner2 = GetNode<Spawner>("%TestSpawner2");

		audioManager = GetNode<AudioManager>("AudioManager");
		audioManager.PlayRandomAmbiance();

		LocationLabel locationLabel = GetNode<LocationLabel>("%LocationLabel");
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