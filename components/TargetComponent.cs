using Godot;

public partial class TargetComponent : NavigationAgent3D {
	public Vector3 VelocityToTarget { get; }
	public bool HasTarget => currentTarget != null;
	public bool DoesNotHaveTarget => currentTarget == null;
	public CharacterBody3D currentTarget;

	public void SetTarget(CharacterBody3D target) {
		TargetPosition = target.Position;
		currentTarget = target;
	}

	public void RemoveTarget() {
		currentTarget = null;
	}

	public void updateTarget() {
		TargetPosition = currentTarget.Position;
	}

	public Vector3 GetTarget() => TargetPosition;
}