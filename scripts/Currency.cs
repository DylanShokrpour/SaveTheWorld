using Godot;

[GlobalClass]
public partial class Currency : Node {
	[Signal]
	public delegate void OnCurrencyChangeEventHandler();

	[Signal]
	public delegate void OnBrokeEventHandler(); // Blud is broke :skull:

	int currency;

	public void AddCurrency(int Amount) {
		currency += Amount;
		EmitSignal(SignalName.OnCurrencyChange);
	}

	public void RemoveCurrency(int Amount) {
		currency -= Amount;
		EmitSignal(SignalName.OnCurrencyChange);
		if (currency <= 0) EmitSignal(SignalName.OnBroke);
	}

	public void SetCurrency(int Amount) {
		currency = Amount;
		if (currency <= 0) EmitSignal(SignalName.OnBroke);
	}

	public int getCurrency() => currency;
}