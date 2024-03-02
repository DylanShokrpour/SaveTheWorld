using Godot;

public partial class GeneratorComponent : Node3D {
	[Export] CurrentType currencyType;
	[Export] int waitTime;
	[Export] bool oneShot;
	Character character;
	Currency editedCurrency;
	Timer currencyTimer;
	Food food;

	public GeneratorComponent(Character owner) {
		character = owner;
	}

	public override void _Ready() {
		DetectCurrencyType();
		ConfigureTimer();
	}

	void DetectCurrencyType() {
		switch (currencyType) {
			case CurrentType.Food:
				editedCurrency = character.food; // ASK THE VANGUARD MASTER TO SEE 
				break;
		}
	}

	void ConfigureTimer() {
		currencyTimer = new();
		currencyTimer.WaitTime = waitTime;
		currencyTimer.OneShot = oneShot;
		AddChild(currencyTimer);
		currencyTimer.Start();
	}

	enum CurrentType {
		Food
	}
}