using Godot;

public partial class Building : StaticBody3D {
	[Export] int damage;
	[Export] int health;
	DetectionComponent detectionComponent;
	Timer attackTimer;

	public override void _Ready() {
		detectionComponent = GetNode<DetectionComponent>("DetectionComponent");
		detectionComponent.ValidBodyEntered += OnDetected;
	}

	public void OnDetected(CharacterBody3D target) {
		if (target is Goon goon) Attack(goon);
	}

	public void Attack(Goon goon) {
		goon.healthComponent.TakeDamage(damage);
	}

	public void ConfigureTimer(Goon goon) {
		attackTimer = new Timer();
		attackTimer.WaitTime = 5;

		void OnAttackTimerOnTimeout() {
			// local function
			Attack(goon);
		}

		attackTimer.Timeout += OnAttackTimerOnTimeout;
		AddChild(attackTimer);
		attackTimer.Start();
	}
}