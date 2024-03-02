using Godot;

public partial class Character : CharacterBody3D {
	const int SPEED = 10;
	const int ACCELERATION = 10;
	const int sensitivity = 5;
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
		food.OnCurrencyChange += updateLabel;
	}

	public override void _PhysicsProcess(double delta) {
		Vector2 inputDir =
			Input.GetVector("movement_left", "movement_right", "movement_forward", "movement_backward");
		Vector3 direction = head.GlobalTransform.Basis *
		                    new Vector3(inputDir.X * sensitivity, 0, inputDir.Y * sensitivity);
		//implement delta into movement
		if (direction != Vector3.Zero) {
			velocity.X = direction.X * SPEED;
			velocity.Z = direction.Z * SPEED;
		}
		else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, ACCELERATION);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, ACCELERATION);
		}

		if (Input.IsActionPressed("movement_jump") && IsOnFloor()) velocity.Y = jumpPower;
		// Gravity (considering frame rate independence)
		velocity.Y -= 5.8f * (float)delta;
		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event) {
		//Input.MouseMode = Input.MouseModeEnum.Captured;
		//Input.MouseMode = Input.MouseModeEnum.Confined;

		if (@event is InputEventMouseMotion mouseMotion) {
			head.RotateY(-mouseMotion.Relative.X * 0.01f);
			camera.RotateX(-mouseMotion.Relative.Y * 0.01f);
			Vector3 cameraRot = camera.Rotation;
			cameraRot.X = Mathf.Clamp(cameraRot.X, Mathf.DegToRad(-80f), Mathf.DegToRad(80f));
			camera.Rotation = cameraRot;
			//lookDir.Rotation = camera.Rotation;
		}
	}

	void updateLabel() {
		foodLabel.Text = "Food: " + food.getCurrency();
	}

	public void AddResource(Currency resource) {
		//food.AddCurrency(resource);
	}
}