using System;
using Godot;

public partial class Stock : Node {
	[Export] int baseValue;
	[Export] float changeRate; //like velocity 
	[Export] float changeInChangeRate; //like acceleration
	int currentValue; // like Position
	int randN;

	public override void _Ready() {
		currentValue = baseValue;
	}

	public void ChangeStockValue() {
		randN = GD.RandRange(-1, 1);
		currentValue = (int)(currentValue + randN * changeRate);
		randN = GD.RandRange(-1, 1);
		changeRate = (int)(changeRate + randN * changeInChangeRate);
	}
}