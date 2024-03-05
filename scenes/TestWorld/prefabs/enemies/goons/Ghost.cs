using System;
using Godot;

public partial class Ghost : Node3D {
	[Export] float sineAmplitude;
	[Export] float sineFrequency = 1;
	float time;
	Goon parent;

	public override void _Ready() {
		parent = GetParent<Goon>();
	}

	//for the actually ghost mechnaic well lol it is in the block componenet I KNOW I KNOW DONT YELL AT ME please
	public override void _Process(double delta) {
		time += (float)delta;
		float nextY = parent.GlobalPosition.Y + sineAmplitude * (float)Math.Sin(time * sineFrequency);
		parent.GlobalPosition = parent.GlobalPosition with {
				Y = nextY
		};
	}
}