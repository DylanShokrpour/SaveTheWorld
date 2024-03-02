using Godot;
using Godot.Collections;

public partial class AudioManager : Node {
	[Export] public Array<AudioStream> ambiance;
	[Export] public Array<AudioStream> epicAmbiance;
	[Export] public AudioStream bossMusic;
	[Export] public AudioStream victorySound;
	[Export] public AudioStream defeatSound;
	AudioStreamPlayer player;

	public override void _Ready() {
		player = GetNode<AudioStreamPlayer>("CurrentMusic");
	}


	public void PlayStream(AudioStream stream) {
		player.Stop();
		player.Stream = stream;
		player.Play();
	}

	public void PlayRandomAmbiance() {
		AudioStream stream = ambiance.PickRandom();
		PlayStream(stream);
	}

	public void PlayRandomEpicAmbiance() {
		AudioStream stream = epicAmbiance.PickRandom();
		PlayStream(stream);
	}
}