using Godot;

public partial class Spawner : Node3D {
	[Export] Path3D path3D;

	public override void _Ready() {
		// practically copy pasted from base.
		const int FIRST_POINT = 0;
		Vector3 position = path3D.GlobalPosition;
		position += path3D.Curve.GetPointPosition(FIRST_POINT);
		GlobalPosition = position;
	}

	public void SpawnGoon(PackedScene goonPrefab) {
		var goon = goonPrefab.Instantiate<Goon>();
		AutoPathFollow autoPathFollow = new(goon.speed);
		path3D.AddChild(autoPathFollow);
		goon.autoPathFollow = autoPathFollow;
		autoPathFollow.AddChild(goon);
	}
}