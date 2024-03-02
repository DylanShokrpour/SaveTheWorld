using System;
using Godot;

public partial class Stock : Node {
	[Export] int baseValue;
	[Export] float changeRate; //like velocity 
	[Export] float changeInChangeRate; //like acceleration
	int currentValue; // like Position
	Random rand;
	int randN;

	public override void _Ready() {
		currentValue = baseValue;
		rand = new();
	}

	public void ChangeStockValue() {
		randN = rand.Next(-1, 1);
		currentValue = (int)(currentValue + randN * changeRate);
		randN = rand.Next(-1, 1);
		changeRate = (int)(changeRate + randN * changeInChangeRate);
	}
}