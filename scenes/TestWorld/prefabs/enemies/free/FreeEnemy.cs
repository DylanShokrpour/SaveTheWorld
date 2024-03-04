using Godot;
using GodotExtensions.extensions.node;

public partial class FreeEnemy : CharacterBody3D {
	[Export] public int damage = 10;
	[Export] public bool shooting;
	//DONT FORGET MAKING INVESTMENTS IN A STOCK MARKET (2/15/2024)// OK OK I GET IT JEEZ I WILL WORK ON IT NOW PAST ME (2/20/2024)// YOU SAID YOU WILL WORK ON IT BUT IT IS  (2/24/2024) AND STILL NOTHING DAMIR 
	[Export] float speed = 5f;
	HealthComponent health;
	float gravity = 6f;
	DetectionComponent detection;
	DetectionComponent attackArea;
	TargetedPathFinderComponent targetPathFinder;
	Vector3 velocity = Vector3.Zero;

	public override void _Ready() {
		targetPathFinder = GetNode<TargetedPathFinderComponent>("TargetedPathFinderComponent");

		detection = GetNode<DetectionComponent>("%DetectionComponent");
		detection.ValidBodyEntered += Target;
		detection.ValidBodyExited += RemoveTarget;

		health = GetNode<HealthComponent>("%HealthComponent");
		health.OnHealthChanged += OnDeath;

		var rangedAttackComponent = GetNodeOrNull<RangedAttackComponent>("RangedAttackComponent");
		if (rangedAttackComponent != null)
			rangedAttackComponent.targetType = typeof(Character);
	}

	void Target(CharacterBody3D target) {
		if (target is not Character)
			return;
		targetPathFinder.Target = target;
	}

	void RemoveTarget() {
		targetPathFinder.Target = null;
	}

	async void OnDeath() {
		await this.WaitForSeconds(1);
		QueueFree();
	}

	public override void _Process(double delta) {
		Velocity = Velocity with { Y = Velocity.Y - gravity * (float)delta };
		MoveAndSlide();
		if (shooting)
			return;
		if (!targetPathFinder.HasTarget)
			return;
		Vector3 nextLocation = targetPathFinder.NextPathPosition;
		//Vector3 VelocityToTarget = (nextLocation - GlobalPosition).Normalized();
		Velocity = Velocity with { X = nextLocation.X - GlobalPosition.X };
		Velocity = Velocity with { Z = nextLocation.Z - GlobalPosition.Z };
		//velocity = new Vector3(GlobalPosition.X, -gravity, GlobalPosition.Z);
		velocity = Velocity * speed;
	}
}