using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotExtensions.extensions.node;

public partial class StockMarket : Node {
	List<Stock> stocks = new();
	Timer timeToChangeStocks;
	[Export] int WaitTime;

	public override void _Ready() {
		stocks = this.GetDescendantsWhichAreA<Stock>().ToList();
		ConfigureTimer();
	}

	void ConfigureTimer() {
		timeToChangeStocks = new Timer();
		timeToChangeStocks.WaitTime = WaitTime;
		AddChild(timeToChangeStocks);
		timeToChangeStocks.Start();
		timeToChangeStocks.Timeout += ChangeStockPrices;
	}

	void ChangeStockPrices() {
		foreach (Stock stock in stocks) stock.changeStockValue();
	}
	//for each ( Loser d in ryanMemearpour.getDescendantsWhichAreA<Loser>(){ d.queueFree(); } :(
}