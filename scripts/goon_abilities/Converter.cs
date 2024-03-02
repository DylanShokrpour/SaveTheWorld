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
			if (building.GetFirstChildOfClass<PathFollowBuilding>() != null) {
				isConverting = true;
				Convert(building.GetFirstChildOfClass<BlockComponent>(),
					building.GetFirstChildOfClass<PathFollowBuilding>());
			}
	}

	async void Convert(BlockComponent block, PathFollowBuilding building) {
		GetParent<Goon>().path.speed = 0;
		building.ChangePathSpeed(0);
		await this.YieldForSeconds(timeToConvert);
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