using System;
using System.Threading.Tasks;
using Godot;
using GodotExtensions.extensions.node;

public partial class Bullet : RigidBody3D {
	[Export] public float speed;
	public Vector3 targetPosition;
	DetectionComponent detectionComponent;
	public int damage;
	public Type targetType;

	public void OnCreated(Type type) {
		targetType = type;
		detectionComponent = GetNode<DetectionComponent>("DetectionComponent");
		detectionComponent.ValidBodyEntered += BodyEntered;
		LinearVelocity = (targetPosition - GlobalPosition) * 10;
		NotHit();
	}

	public async Task NotHit() {
		await this.YieldForSeconds(4);
		DeleteBullet();
	}

	public async void DeleteBullet() {
		if (HasNode("AreaDamageComponent")) {
			var areaDamageComponent = GetNode<AreaDamageComponent>("AreaDamageComponent");
			areaDamageComponent.Impact();
		}

		QueueFree();
	}


	public void BodyEntered(Node3D body) {
		if (body.GetType() == targetType) {
			var targetHealth = body.GetFirstChildOfClass<HealthComponent>();
			targetHealth.TakeDamage(damage);
			DeleteBullet();
		}
	}
}