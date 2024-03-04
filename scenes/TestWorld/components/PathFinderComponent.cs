using Godot;

/// <summary>
///     Use SetPhysicsProcess(false) to stop the pathfinding.
/// </summary>
public partial class PathFinderComponent : NavigationAgent3D {
	public enum UpdateRate {
		Every_Frame,
		Every_Other_Frame,
		Every_5_Frames,
		Every_30_Frames,
		Every_60_Frames
	}

	[Export] public UpdateRate updateRate = UpdateRate.Every_Frame;

	/// <summary>
	///     The last computed path position.
	/// </summary>
	public Vector3 NextPathPosition { get; private set; }

	public override void _PhysicsProcess(double delta) {
		if (!ShouldProcessNavigation())
			return;

		NextPathPosition = GetNextPathPosition();
	}

	bool ShouldProcessNavigation() {
		if ((GetFrameCount() % GetUpdateRate()) != 0)
			return false;
		return true;

		static int GetFrameCount() => (int)Engine.GetPhysicsFrames();
	}

	int GetUpdateRate() {
		return updateRate switch {
			UpdateRate.Every_Frame => 1,
			UpdateRate.Every_Other_Frame => 2,
			UpdateRate.Every_5_Frames => 5,
			UpdateRate.Every_30_Frames => 30,
			UpdateRate.Every_60_Frames => 60,
			_ => 1
		};
	}
}