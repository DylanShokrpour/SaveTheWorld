using Godot;
using Godot.Collections;

public partial class NaturalResource : StaticBody3D {
	public enum NaturalResourceType {
		Food
	}

	[Export] public NaturalResourceType type;

	[Export] public Array<PackedScene> resourceList;

	[Export] public Array<Currency> goods;

	[Export] int goodsReceived;

	void MatchCurrency() {
		switch (type) {
			case NaturalResourceType.Food:
				resourceList = new() {
					GD.Load<PackedScene>("res://Scenes/Currency/Food.tscn")
				};
				break;
		}
	}

	public override void _Ready() { }

	void OnHarvest(Character player) {
		for (var i = 0; i < resourceList.Count; i++) {
			var good = resourceList[i].Instantiate<Currency>();
			goods.Add(good);
			player.AddResource(goods[i]);
		}
	}
}