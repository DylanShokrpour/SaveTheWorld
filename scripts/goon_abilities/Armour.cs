using Godot;
using GodotExtensions.extensions.node;

public partial class Armour : StaticBody3D {
	[Export] float amourStrength = 1;
	HealthComponent parentHealthComponent;

	public override void _Ready() {
		parentHealthComponent = GetParent().GetFirstChildOfClass<HealthComponent>();
		ActivateArmour();
	}

	void ActivateArmour() {
		parentHealthComponent.damageModifier =
			1 / amourStrength; //i wanted it so increasing amourStrength would make damage decrease slower
	}
}