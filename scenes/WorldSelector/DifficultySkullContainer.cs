using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GodotExtensions.extensions.node;

public partial class DifficultySkullContainer : HBoxContainer
{
	public List<TextureRect> difficultySkullRects;
	
	public override void _Ready() {
		difficultySkullRects = this.GetDescendants<TextureRect>().ToList();
		for (int i = 0; i < difficultySkullRects.Count; i++) {
			difficultySkullRects[i].Visible = false;
		}
	}
	public void UpdateVisuals(double difficulty) {
		GD.Print("YAY! I AM CALLED!");
		for (int i = 0; i < difficulty; i++) {
			difficultySkullRects[i].Visible = true;
		}
		for (int i = difficultySkullRects.Count - 1; i >= difficulty; i--) {
			difficultySkullRects[i].Visible = false;
		}
	}
}
