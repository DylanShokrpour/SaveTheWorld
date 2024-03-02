using Godot;
using GodotExtensions.extensions.node;

public partial class Converter : Area3D {
	bool isConverting;
	Timer timer;
	[Export] public int timeToConvert;

	public override void _Ready() {
		ConfigureTimer();
		BodyEntered += FindBody;
	}

	void FindBody(Node3D body) {
		if (body is Building building && !isConverting)
			if (building.GetFirstChildOfClass<PathFollowBuilding>() != null) {
				isConverting = true;
				convert(building.GetFirstChildOfClass<BlockComponent>(),
					building.GetFirstChildOfClass<PathFollowBuilding>());
			}
	}

	async void convert(BlockComponent block, PathFollowBuilding building) {
		GetParent<Goon>().path.Speed = 0;
		building.ChangePathSpeed(0);
		await this.YieldForSeconds(timeToConvert);
		block.targetType = typeof(Building);
		building.ChangePathSpeed(building.speed);
		isConverting = false;
	}


	void ConfigureTimer() {
		timer = new Timer();
		timer.WaitTime = timeToConvert;
		AddChild(timer);
		timer.Start();
	}
}