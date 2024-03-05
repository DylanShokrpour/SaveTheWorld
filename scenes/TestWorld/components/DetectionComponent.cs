using Godot;

public partial class DetectionComponent : Area3D {
	[Signal]
	public delegate void ValidBodyEnteredEventHandler(CharacterBody3D body);

	[Signal]
	public delegate void ValidBodyExitedEventHandler();

	[Signal]
	public delegate void ValidBulletDetectedEventHandler(Bullet bullet);

	public CharacterBody3D target;
	//[Signal] public delegate void ValidEnemyDetectedEventHandler(CharacterBody3D target);

	public override void _Ready() {
		BodyEntered += CheckBodyType;
		BodyEntered += BulletEntered;
		//BodyEntered += ValidEnemyDetected;
		BodyExited += CheckBodyTypeLeft;
	}

	void CheckBodyType(Node3D body) {
		if (body is CharacterBody3D body3D && (body3D != Owner)) {
			target = body3D;
			EmitSignal(SignalName.ValidBodyEntered, body3D);
		}
	}


	void CheckBodyTypeLeft(Node3D body) {
		if (body == target) {
			target = null;
			EmitSignal(SignalName.ValidBodyExited);
		}
	}

	void BulletEntered(Node3D body) {
		if (body is Bullet bullet)
			EmitSignal(SignalName.ValidBulletDetected, bullet);
	}
}