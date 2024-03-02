using Godot;

[GlobalClass]
public partial class Currency : Node {

	[Signal]
	public delegate void OnBrokeEventHandler(); // Blud is broke :skull:

	[Signal]
	public delegate void OnCurrencyChangeEventHandler();

	int currency;

	public void AddCurrency(int amount) {
		currency += amount;
		EmitSignal(SignalName.OnCurrencyChange);
	}

	public void RemoveCurrency(int amount) {
		currency -= amount;
		EmitSignal(SignalName.OnCurrencyChange);
		if (currency <= 0)
			EmitSignal(SignalName.OnBroke);
	}

	public void SetCurrency(int amount) {
		currency = amount;
		if (currency <= 0)
			EmitSignal(SignalName.OnBroke);
	}

	public int GetCurrency() => currency;
}