using Godot;

public partial class Base : Building {
	[Export] public Path3D path;
	RangedAttackComponent rangedAttackComponent;
	DetectionComponent detectionComponent;
	HealthComponent healthComponent;

	public override void _Ready() {
		healthComponent = GetNode<HealthComponent>("%HealthComponent");
		detectionComponent = GetNode<DetectionComponent>("DetectionComponent");
		detectionComponent.ValidBodyEntered += Hit;
		
		rangedAttackComponent = GetNode<RangedAttackComponent>("RangedAttackComponent");
		rangedAttackComponent.targetType = typeof(Goon);
		
		int lastPoint = path.Curve.PointCount - 1;
		Position = path.Curve.GetPointPosition(lastPoint);
	}

	void Hit(CharacterBody3D character) {
		if (character is Goon goon) {
			healthComponent.TakeDamage(goon.damage);
			goon.HealthComponent.SetHealth(0);
		}
	}
}