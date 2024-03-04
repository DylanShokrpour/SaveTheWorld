using Godot;

/// <summary>
/// You can assign a target to this component and it will always path find to it.
/// </summary>
public partial class TargetedPathFinderComponent : PathFinderComponent {
	[Signal] public delegate void OnTargetLostEventHandler();

	public Node3D Target { get; set; }

	public bool HasTarget => IsInstanceValid(Target);

	bool accountedForTargetLoss;

	public override void _PhysicsProcess(double delta) {
		if (!HasTarget) {
			// Stupid convert to event driven.
			if (accountedForTargetLoss)
				return;
			accountedForTargetLoss = true;
			
			EmitSignal(SignalName.OnTargetLost);
			return;
		}

		accountedForTargetLoss = false;

		// -----------------------------
		
		TargetPosition = Target.GlobalPosition;
		base._PhysicsProcess(delta);
	}
}