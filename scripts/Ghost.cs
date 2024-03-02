using System;
using Godot;

public partial class Ghost : Node3D {
	[Export] float sineAmplitude;
	int incrementor;
	Goon parent;

	public override void _Ready() {
		parent = GetParent<Goon>();
	}

	//for the actually ghost mechnaic well lol it is in the block componenet I KNOW I KNOW DONT YELL AT ME please
	public override void _Process(double delta) {
		incrementor++;
		parent.GlobalPosition =
			parent.GlobalPosition with {
				Y = parent.GlobalPosition.Y + sineAmplitude * (float)Math.Sin(incrementor * delta)
			};
	}
}