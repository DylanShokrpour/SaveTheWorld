using Godot;

public partial class GeneratorComponent : Node3D {
	Character character;
	[Export] CurrentType currencyType;
	Currency editedCurrency;
	Timer currencyTimer;
	[Export] int waitTime;
	[Export] bool oneShot;
	Food food;

	public GeneratorComponent(Character owner) {
		character = owner;
	}

	public override void _Ready() {
		detectCurrencyType();
		configureTimer();
	}

	void detectCurrencyType() {
		switch (currencyType) {
			case CurrentType.Food:
				editedCurrency = character.food; // ASK THE VANGUARD MASTER TO SEE 
				break;
		}
	}

	void configureTimer() {
		currencyTimer = new Timer();
		currencyTimer.WaitTime = waitTime;
		currencyTimer.OneShot = oneShot;
		AddChild(currencyTimer);
		currencyTimer.Start();
	}

	enum CurrentType {
		Food
	}
}