using Godot;
using GodotExtensions.extensions.node;

public partial class Weapon : StaticBody3D {
	[Export] public int damage = 10;
	[Export] public int damageToTrackBuildingsModifer = 2;
	[Export] bool isLongReach;
	Area3D TrackDetector;

	public override void _Ready() {
		//ehh maybe more will come to this script later
		GetParent<Goon>().damage += damage;
		TrackDetector = GetNode<Area3D>("TrackBuildingDetector");
		TrackDetector.BodyEntered += attack;
	}

	void attack(Node3D body) {
		if ((body.GetFirstChildOfClass<HealthComponent>() != null) && body is Building) {
			HealthComponent healthComponent = body.GetFirstChildWhichIsA<HealthComponent>();
			healthComponent.TakeDamage(damage * damageToTrackBuildingsModifer);
		}
	}
}