using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotExtensions.extensions.node;

public partial class AudioManager : Node {
	[Signal]
	public delegate void OnMusicChangeEventHandler();

	public List<AudioStreamPlayer> ambiance;
	public List<AudioStreamPlayer> epicAmbiance;
	public AudioStreamPlayer bossMusic;
	public AudioStreamPlayer victorySound;
	public AudioStreamPlayer defeatSound;
	AudioStreamPlayer currentMusic;

	public override void _Ready() {
		currentMusic = GetNode<AudioStreamPlayer>("CurrentMusic");
		bossMusic = GetNode<AudioStreamPlayer>("BossMusic");
		defeatSound = GetNode<AudioStreamPlayer>("DefeatSound");
		victorySound = GetNode<AudioStreamPlayer>("VictorySound");
		//Stream stream; Useless thing bro Why is it here well why not bozo NO PLEASE ENLIGTHEN ME WHY NOT
		ambiance = GetAmbiance();
		epicAmbiance = GetEpicAmbiance();
	}

	List<AudioStreamPlayer> GetAmbiance() =>
		GetNode<Node3D>("Ambiance").GetDescendantsWhichAreA<AudioStreamPlayer>().ToList();

	List<AudioStreamPlayer> GetEpicAmbiance() =>
		GetNode<Node3D>("EpicAmbiance").GetDescendantsWhichAreA<AudioStreamPlayer>().ToList();

	public void PlayVictorySound() {
		currentMusic.Stop();
		currentMusic = victorySound;
		currentMusic.VolumeDb = -10;
		currentMusic.Play();
		EmitSignal(SignalName.OnMusicChange);
	}

	public void PlayDefeatSound() {
		currentMusic.Stop();
		currentMusic = defeatSound;
		currentMusic.VolumeDb = -10;
		currentMusic.Play();
		EmitSignal(SignalName.OnMusicChange);
	}

	public void PlayBossMusic() {
		currentMusic.Stop();
		currentMusic = bossMusic;
		currentMusic.VolumeDb = -10;
		currentMusic.Play();
		EmitSignal(SignalName.OnMusicChange);
		currentMusic.Finished += () => PlayBossMusic();
	}

	public void PlayAmbiance() {
		currentMusic.Stop();
		GD.Print("AMIABCE");
		int randN = new Random().Next(0, ambiance.Count);
		currentMusic = ambiance[randN];
		currentMusic.VolumeDb = -10;
		currentMusic.Play();
		EmitSignal(SignalName.OnMusicChange);
		currentMusic.Finished += () => PlayAmbiance();
	}

	public void PlayEpicAmbiance() {
		currentMusic.Stop();
		int randN = new Random().Next(0, epicAmbiance.Count - 1);
		currentMusic = epicAmbiance[randN];
		currentMusic.VolumeDb = -10;
		currentMusic.Play();
		EmitSignal(SignalName.OnMusicChange);
		currentMusic.Finished += () => PlayEpicAmbiance();
	}
}