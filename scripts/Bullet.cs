using System;
using System.Threading.Tasks;
using Godot;
using GodotExtensions.extensions.node;

public partial class Bullet : RigidBody3D {
	public Vector3 targetPosition;
	DetectionComponent detectionComponent;
	public int damage;
	[Export] public float speed;
	public Type targetType;

	public void onCreated(Type type) {
		targetType = type;
		detectionComponent = GetNode<DetectionComponent>("DetectionComponent");
		detectionComponent.ValidBodyEntered += BodyEntered;
		LinearVelocity = (targetPosition - GlobalPosition) * 10;
		NotHit();
	}

	public async Task NotHit() {
		await this.YieldForSeconds(4);
		deleteBullet();
	}

	public async void deleteBullet() {
		if (HasNode("AreaDamageComponent")) {
			AreaDamageComponent areaDamageComponent = GetNode<AreaDamageComponent>("AreaDamageComponent");
			areaDamageComponent.impact();
		}

		QueueFree();
	}


	public void BodyEntered(Node3D body) {
		if (body.GetType() == targetType) {
			HealthComponent targetHealth = body.GetFirstChildOfClass<HealthComponent>();
			targetHealth.TakeDamage(damage);
			deleteBullet();
		}
	}
}