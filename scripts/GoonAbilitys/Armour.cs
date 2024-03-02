using Godot;
using GodotExtensions.extensions.node;

public partial class Armour : StaticBody3D {
	HealthComponent parentHealthComponent;
	[Export] float amourStrength;

	public override void _Ready() {
		parentHealthComponent = GetParent().GetFirstChildOfClass<HealthComponent>();
		ActivateArmour();
	}

	public void ActivateArmour() {
		parentHealthComponent.damageModifier =
			1 / amourStrength; //i wanted it so increasing amourStrength would make damage decrease slower
	}
}