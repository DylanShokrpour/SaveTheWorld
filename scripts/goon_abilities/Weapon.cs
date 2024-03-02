using Godot;
using GodotExtensions.extensions.node;

public partial class Weapon : StaticBody3D {
	[Export] public int damage = 10;
	[Export] public int damageToTrackBuildingsModifer = 2;
	[Export] bool isLongReach;
	Area3D trackDetector;

	public override void _Ready() {
		//ehh maybe more will come to this script later
		GetParent<Goon>().damage += damage;
		trackDetector = GetNode<Area3D>("TrackBuildingDetector");
		trackDetector.BodyEntered += Attack;
	}

	void Attack(Node3D body) {
		if ((body.FindFirstChildOfClass<HealthComponent>() != null) && body is Building) {
			var healthComponent = body.FindFirstChild<HealthComponent>();
			healthComponent.TakeDamage(damage * damageToTrackBuildingsModifer);
		}
	}
}