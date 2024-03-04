using System.Diagnostics;
using Godot;

[GlobalClass]
public partial class Currency : Node {

	[Signal]
	public delegate void OnBrokeEventHandler(); // Blud is broke :skull:

	[Signal]
	public delegate void OnCurrencyChangedEventHandler();
	public int Amount
	{
		get => amount;
		set
		{
			amount = value;
			if (amount <= 0)
				EmitSignal(SignalName.OnBroke);
			EmitSignal(SignalName.OnCurrencyChanged);
		}
	}
	int amount;

	public void AddAmount(int amountToAdd) {
		Debug.Assert(amountToAdd > 0, "Amount must be positive");
		amount += amountToAdd;
	}

	public void RemoveAmount(int amountToRemove) {
		Debug.Assert(amountToRemove > 0, "Amount must be positive");
		amount -= amountToRemove;
	}
}