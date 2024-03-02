using Godot;

public partial class Teleporter : Node3D {
	PathFollow3D path;
	[Export] float teleportDistance;
	Timer teleportTimer;


	public override void _Ready() {
		path = GetParent<Goon>().path;
		GetParent<Goon>().healthComponent.OnHealthChange += Teleport;
		ConfigureTimer();
	}

	void Teleport() {
		path.Progress = path.Progress + teleportDistance;
	}

	void ConfigureTimer() {
		// IMPLEMENT LATER
		teleportTimer = new Timer();
		teleportTimer.WaitTime = 1;
		AddChild(teleportTimer);
		teleportTimer.Start();
	}
}