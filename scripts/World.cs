using Godot;

public partial class World : Node3D {
	// lol the export doesn't even work but Hoy Hoy HOy
	[Export] Wave wave1;
	[Export] Wave wave2;

	LocationLabel locationLabel;

	//[Export] RichTextLabel introduceLevel; // This is a label that will be used to introduce the level(I AM A TROLL OK)
	//Wave wave = new Wave();
	Spawner spawner2;
	WaveManager waveManager;
	AudioManager audioManager;


	public override void _Ready() {
		//introduceLevel = GetNode<RichTextLabel>("IntroduceLevel");
		audioManager = GetNode<AudioManager>("AudioManager");
		spawner2 = GetNode<Spawner>("%TestSpawner2");
		waveManager = GetNode<WaveManager>("WaveManager");
		locationLabel = GetNode<LocationLabel>("%LocationLabel");
		GD.Print(locationLabel.Name);
		locationLabel.Text = Name;
		locationLabel.Activate();
		audioManager.PlayAmbiance();
		waveManager.newWaveStart += CheckForMusicChange;
	}

	void CheckForMusicChange() {
		if ((waveManager.totalWaves / 2) == waveManager.waveCount) audioManager.PlayEpicAmbiance();
	}
}