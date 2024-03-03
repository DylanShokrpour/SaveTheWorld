using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotExtensions.extensions.node;

public partial class StockMarket : Node {
	[Export] int WaitTime;
	List<Stock> stocks = new();
	Timer timeToChangeStocks;

	public override void _Ready() {
		stocks = this.GetDescendants<Stock>().ToList();
		ConfigureTimer();
	}

	void ConfigureTimer() {
		timeToChangeStocks = new();
		timeToChangeStocks.WaitTime = WaitTime;
		AddChild(timeToChangeStocks);
		timeToChangeStocks.Start();
		timeToChangeStocks.Timeout += ChangeStockPrices;
	}

	void ChangeStockPrices() {
		foreach (Stock stock in stocks)
			stock.ChangeStockValue();
	}
}