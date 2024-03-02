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
		detectionComponent.ValidBodyEntered += OnBodyEntered;
		LinearVelocity = (targetPosition - GlobalPosition) * 10;
		_ = NotHit();
	}

	async Task NotHit() { // prevents from going into void
		await this.WaitForSeconds(4);
		DeleteBullet();
	}

	public void DeleteBullet() {
		if (HasNode("AreaDamageComponent")) {
			var areaDamageComponent = GetNode<AreaDamageComponent>("AreaDamageComponent");
			areaDamageComponent.Impact();
		}

		QueueFree();
	}


	void OnBodyEntered(Node3D body) {
		if (body.GetType() == targetType) {
			var targetHealth = body.FindFirstChildOfClass<HealthComponent>();
			targetHealth.TakeDamage(damage);
			DeleteBullet();
		}
	}
}