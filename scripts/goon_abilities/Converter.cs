using Godot;
using GodotExtensions.extensions.node;

public partial class Converter : Area3D {
	[Export] public int timeToConvert;
	bool isConverting;
	Timer timer;

	public override void _Ready() {
		ConfigureTimer();
		BodyEntered += FindBody;
	}

	void FindBody(Node3D body) {
		if (body is Building building && !isConverting)
			if (building.FindFirstChildOfClass<PathFollowBuilding>() != null) {
				isConverting = true;
				Convert(building.FindFirstChildOfClass<BlockComponent>(),
					building.FindFirstChildOfClass<PathFollowBuilding>());
			}
	}

	async void Convert(BlockComponent block, PathFollowBuilding building) {
		GetParent<Goon>().autoPathFollow.speed = 0;
		building.ChangePathSpeed(0);
		await this.WaitForSeconds(timeToConvert);
		block.targetType = typeof(Building);
		building.ChangePathSpeed(building.speed);
		isConverting = false;
	}


	void ConfigureTimer() {
		timer = new();
		timer.WaitTime = timeToConvert;
		AddChild(timer);
		timer.Start();
	}
}