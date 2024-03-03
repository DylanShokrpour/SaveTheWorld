using Godot;
using Godot.Collections;

public partial class WorldMenu : Node {
	PackedScene
		worldScene; // btw i was writing this and all of a sudden i got a text from Ryan showing his delicous food and man oh man that food looks scrumptious

	[Export] Array<Texture2D> images;
	[Export] AudioStream music;
	[Export] public Color color;
	[Export] public string font = "[center]"; //NOT A FONT BUT CRY ABOUT IT
	[Export] int specialImageRarety = 10;

	public Texture2D getAnImage() {
		font = font + "[color=#" + color.ToHtml() + "]";
		Texture2D image = images[GD.RandRange(0, images.Count - 2)]; //-2 since we exclude the rare image
		if (GD.RandRange(0, specialImageRarety) == 0) image = images[images.Count - 1];
		return image;
	}

	public AudioStream getMusic() => music;
}