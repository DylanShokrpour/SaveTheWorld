using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Godot;
using Godot.Collections;
using GodotExtensions.extensions.node;

public partial class WorldSelector : Control {
	[Signal]

	public delegate void onLevelChangeEventHandler(bool direction);
	[Export] Godot.Collections.Array<Texture2D> MapDiffcultySkulls;
	List<WorldMenu> worldMenus;
	int currentMapSelectedIndex;
	TextureRect backGround;
	AudioStreamPlayer audioPlayer;
	RichTextLabel levelName;
	DifficultySkullContainer difficultySkullContainer;
	HSlider difficultySlider;
	TextureRect mapDifficultySkull;

	public override void _Ready() {
		worldMenus = this.GetDescendants<WorldMenu>().ToList();
		backGround = GetNode<TextureRect>("%CurrentImage");
		audioPlayer = GetNode<AudioStreamPlayer>("%AudioPlayer");
		levelName = GetNode<RichTextLabel>("%LevelName");
		difficultySlider = GetNode<HSlider>("%DiffcultySlider");
		mapDifficultySkull = GetNode<TextureRect>("%MapDifficultySkull"); //imagine :Skull:
		difficultySkullContainer = GetNode<DifficultySkullContainer>("%DifficultySkullContainer");
		difficultySlider.ValueChanged += updateDiffculty;
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
		for(int i = 0; i < worldMenus.Count; i++) {
			//worldMenus[i].Visible = i == currentMapSelectedIndex;
		}
		
		backGround.Texture = worldMenus[currentMapSelectedIndex].getAnImage();
		audioPlayer.Stream = worldMenus[currentMapSelectedIndex].getMusic();
		levelName.Text = worldMenus[currentMapSelectedIndex].font + worldMenus[currentMapSelectedIndex].Name;
		updateMapDifficulty();
		audioPlayer.Play();
	}

	void updateDiffculty(double newValue) {
		GD.Print("YAY! I AM CALLED!; Dude number two: Lol imagine being that desperate to be called the world doesn't revolve around you");
		difficultySkullContainer.UpdateVisuals(newValue);
	}
	void updateMapDifficulty() { // map difficulty is the difficulty of the map(such as parkour and other stuff while diffculty is your standard strength of enemies
		int index = worldMenus[currentMapSelectedIndex].mapDifficulty;
		mapDifficultySkull.Texture = MapDiffcultySkulls[index];
	}
	void addRalph() {
		PackedScene ralph = GD.Load<PackedScene>("res://Ralph.tscn");
		//dont mess with the program ralph. lol blud actually memorized each song and when they play in the move :skull:
		// why idk why not... it was just that i was making this while watching a video about ralph and i was like "hey why not add ralph"(btw the second half of this message was copilot) what the actually story is before copilot inreputed was that rekt it ralph music was playing by the famous Vanguaard master so it made me think about this
	}
}