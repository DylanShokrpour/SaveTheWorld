using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotExtensions.extensions.node;

public partial class WorldSelector : Control {
	[Signal]
	public delegate void onLevelChangeEventHandler(bool direction);

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

	void changeLevel(bool direction) {
		if(currentMapSelectedIndex <= 0 && direction == false) return;
		if(currentMapSelectedIndex == worldMenus.Count - 1 && direction == true) return;
		if (direction == true) {
			currentMapSelectedIndex++;
		}
		else {
			currentMapSelectedIndex--;
		}
		updateVisuals();
	}

	void updateVisuals() {
		backGround.Texture = worldMenus[currentMapSelectedIndex].getAnImage();
		audioPlayer.Stream = worldMenus[currentMapSelectedIndex].getMusic();
		levelName.Text = worldMenus[currentMapSelectedIndex].font + worldMenus[currentMapSelectedIndex].Name;
		audioPlayer.Play();
	}
}