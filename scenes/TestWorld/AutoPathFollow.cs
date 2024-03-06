using System;
using Godot;

public partial class AutoPathFollow : PathFollow3D {
	[Signal] public delegate void PathWrappingEventHandler();

	public enum Direction {
		Forward,
		Backward
	}

	const int PATH_START = 0;
	const int PATH_END = 1;

	Path3D path;
	public float speed;
	Direction direction;

	public AutoPathFollow(string unitName, float speed, Direction direction = Direction.Forward, float? customProgress = null) {
		Name = "AutoPathFollow for " + unitName;
		this.speed = speed;
		this.direction = direction;
		if (customProgress != null)
			Progress = customProgress.Value;
		else // hack.
			Connect(Node.SignalName.Ready, Callable.From(SetProgressRatioOnReady), (uint)ConnectFlags.OneShot);
	}

	void SetProgressRatioOnReady() {
		ProgressRatio = GetProgressRatioFromDirection(direction);
	}

	public override void _EnterTree() {
		path = GetParent<Path3D>();
	}


	static int GetProgressRatioFromDirection(Direction direction) {
		return direction switch {
			Direction.Forward => PATH_START,
			Direction.Backward => PATH_END,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};
	}

	public override void _PhysicsProcess(double delta) {
		float amountToProgress = speed * (float)delta;
		if (direction == Direction.Backward)
			amountToProgress *= -1;

		float nextProgress = Progress + amountToProgress;
		if (GetProgressRatio(nextProgress) is < PATH_START or > PATH_END)
			EmitSignal(SignalName.PathWrapping);
		// no need to wrap here, as its done by godot automatically.
		Progress = nextProgress;
	}

	// copied from godot C++ code.
	float GetProgressRatio(float progress) {
		if (path == null)
			return 0;
		Curve3D curve = path.Curve;
		if (!IsInstanceValid(curve))
			return 0;

		return progress / curve.GetBakedLength();
	}
}