using Godot;

public partial class Base : Building {
	[Export] public Path3D path;
	DetectionComponent detectionComponent;
	HealthComponent healthComponent;

	public override void _Ready() {
		healthComponent = GetNode<HealthComponent>("%HealthComponent");
		detectionComponent = GetNode<DetectionComponent>("DetectionComponent");
		detectionComponent.ValidBodyEntered += Hit;
		int lastPoint = path.Curve.PointCount - 1;
		Position = path.Curve.GetPointPosition(lastPoint);
	}

	public void Hit(CharacterBody3D character) {
		if (character is Goon goon) {
			healthComponent.TakeDamage(goon.damage);
			goon.healthComponent.SetHealth(0);
		}
	}
}