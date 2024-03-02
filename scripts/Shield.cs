using Godot;

public partial class Shield : StaticBody3D {
	HealthComponent shieldHealth;
	DetectionComponent detectionComponent;

	public override void _Ready() {
		shieldHealth = GetNode<HealthComponent>("SHealthComponent");
		detectionComponent = GetNode<DetectionComponent>("SDetectionComponent");
		detectionComponent.ValidBulletDetected += Hit;
	}

	public void Hit(Bullet bullet) {
		shieldHealth.TakeDamage(bullet.damage);
		bullet.deleteBullet();
	}
}