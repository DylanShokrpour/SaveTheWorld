using Godot;

public partial class Base : StaticBody3D {
	DetectionComponent detectionComponent;
	HealthComponent healthComponent;
	[Export] Path3D path;

	public override void _Ready() {
		healthComponent = GetNode<HealthComponent>("%HealthComponent");
		detectionComponent = GetNode<DetectionComponent>("DetectionComponent");
		detectionComponent.ValidBodyEntered += Hit;
		GD.Print(Position);
		int lastPoint = path.Curve.PointCount - 1;
		Position = path.Curve.GetPointPosition(lastPoint);
		GD.Print(Position);
	}

	public void Hit(CharacterBody3D character) {
		if (character is Goon goon) {
			healthComponent.TakeDamage(goon.damage);
			character.QueueFree();
		}
	}
}