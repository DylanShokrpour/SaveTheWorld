using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotExtensions.extensions.node;

public partial class WorldSelector : Control {
	[Signal]
	public delegate void onLevelChangeEventHandler();

	List<WorldMenu> worldMenus;
	int currentMapSelectedIndex;
	TextureRect backGround;
	AudioStreamPlayer audioPlayer;
	RichTextLabel levelName;

	public override void _Ready() {
		worldMenus = this.GetDescendants<WorldMenu>().ToList();
		backGround = GetNode<TextureRect>("%CurrentImage");
		audioPlayer = GetNode<AudioStreamPlayer>("%AudioPlayer");
		levelName = GetNode<RichTextLabel>("%LevelName");
		onLevelChange += changeLevel;
		updateVisuals();
	}

	void changeLevel() {
		currentMapSelectedIndex++;
		updateVisuals();
	}

	void updateVisuals() {
		backGround.Texture = worldMenus[currentMapSelectedIndex].getAnImage();
		audioPlayer.Stream = worldMenus[currentMapSelectedIndex].getMusic();
		levelName.Text = worldMenus[currentMapSelectedIndex].font + worldMenus[currentMapSelectedIndex].Name;
		audioPlayer.Play();
	}
}