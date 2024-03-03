using Godot;

public partial class Teleporter : Node3D {
	[Export] float teleportDistance;
	PathFollow3D path;
	Timer teleportTimer;


	public override void _Ready() {
		path = GetParent<Goon>().path;
		GetParent<Goon>().HealthComponent.OnHealthChanged += Teleport;
		ConfigureTimer();
	}

	void Teleport() {
		path.Progress = path.Progress + teleportDistance;
	}

	void ConfigureTimer() {
		// IMPLEMENT LATER
		teleportTimer = new();
		teleportTimer.WaitTime = 1;
		AddChild(teleportTimer);
		teleportTimer.Start();
	}
}