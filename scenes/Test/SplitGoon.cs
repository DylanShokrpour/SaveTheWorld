using Godot;

public partial class SplitGoon : Node3D {
	[Export] PackedScene prefabGoon;
	[Export] int amount;
	Goon parentGoon;

	public override void _Ready() {
		parentGoon = GetParent<Goon>();
		parentGoon.healthComponent.OnDeath += die;
	}

	void die() {
		for (int i = 0; i < amount; i++) {
			Goon goon = prefabGoon.Instantiate<Goon>();
			GetParent().AddChild(goon);
			goon.GlobalTransform = GlobalTransform;
		}
	}

	void createGoon() { }
}