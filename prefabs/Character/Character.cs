using Godot;

public partial class Character : CharacterBody3D {
	// cache string names for optimization.
	readonly static StringName movementLeft = "movement_left";
	readonly static StringName movementRight = "movement_right";
	readonly static StringName movementForward = "movement_forward";
	readonly static StringName movementBackward = "movement_backward";
	[Export] float speed = 5;
	[Export] float acceleration = 10;
	[Export] float sensitivity = 0.01f;
	public Currency food = new();
	RichTextLabel foodLabel;
	float jumpPower = 10;
	Vector3 velocity;
	Node3D head;
	Camera3D camera;

	public override void _Ready() {
		Input.MouseMode = Input.MouseModeEnum.Captured;
		head = GetNode<Node3D>("%Head");
		camera = GetNode<Camera3D>("%PlayerCamera");
		foodLabel = GetNode<RichTextLabel>("%FoodLabel");
		food.OnCurrencyChanged += UpdateLabel;
		UpdateLabel();
	}

	public override void _PhysicsProcess(double delta) {
		Vector2 inputDir =
			Input.GetVector(movementLeft, movementRight, movementForward, movementBackward);
		Vector3 direction = head.GlobalTransform.Basis *
		                    new Vector3(inputDir.X, 0, inputDir.Y);
		direction = direction.Normalized();
		//implement delta into movement
		if (direction != Vector3.Zero) {
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
		} else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, acceleration);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, acceleration);
		}

		if (Input.IsActionPressed("movement_jump") && IsOnFloor())
			velocity.Y = jumpPower;
		// Gravity (considering frame rate independence)
		velocity.Y -= 5.8f * (float)delta;
		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event) {
		//Input.MouseMode = Input.MouseModeEnum.Captured;
		//Input.MouseMode = Input.MouseModeEnum.Confined;

		if (@event is InputEventMouseMotion mouseMotion) {
			head.RotateY(-mouseMotion.Relative.X * sensitivity);
			camera.RotateX(-mouseMotion.Relative.Y * sensitivity);
			Vector3 cameraRot = camera.Rotation;
			cameraRot.X = Mathf.Clamp(cameraRot.X, Mathf.DegToRad(-80f), Mathf.DegToRad(80f));
			camera.Rotation = cameraRot;
			//lookDir.Rotation = camera.Rotation;
		}
	}

	void UpdateLabel() {
		foodLabel.Text = "Food: " + food.Amount;
	}

	public void AddResource(Currency resource) {
		//food.AddCurrency(resource);
	}
}