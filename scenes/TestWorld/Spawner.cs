using Godot;

public partial class Spawner : Node3D {
	[Export] public Path3D path;
	[Export] public Node spawnNode;

	public void SpawnUnit(Node3D unit, AutoPathFollow autoPathFollow) {
		unit.TreeExiting += autoPathFollow.QueueFree;
		path.AddChild(autoPathFollow, true);

		unit.Position = autoPathFollow.GlobalPosition;
		spawnNode.AddChild(unit, true);

		RemoteTransform3D remoteTransform3D = CreateRemoteTransform(unit.Name);
		autoPathFollow.AddChild(remoteTransform3D, true);
		remoteTransform3D.RemotePath = remoteTransform3D.GetPathTo(unit);
	}

	static RemoteTransform3D CreateRemoteTransform(string unitName) {
		RemoteTransform3D remoteTransform3D = new() {
			Name = $"RemoteTransform3D for {unitName}",
			UseGlobalCoordinates = true,
			UpdateScale = true,
			UpdatePosition = true,
			UpdateRotation = true
		};

		return remoteTransform3D;
	}
}