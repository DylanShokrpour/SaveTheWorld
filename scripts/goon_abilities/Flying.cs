using Godot;

public partial class Flying : Node3D {
	[Export] public float height;

	public override void _Ready() {
		var parent = GetParent<Node3D>();
		parent.GlobalPosition = parent.GlobalPosition with { Y = parent.GlobalPosition.Y + height };
	}
}