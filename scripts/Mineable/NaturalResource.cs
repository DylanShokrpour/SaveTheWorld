using Godot;
using Godot.Collections;

public partial class NaturalResource : StaticBody3D {
	public enum NaturalResourceType {
		Food
	}

	[Export] int goodsReceived;
	[Export] public NaturalResourceType type;

	[Export] public Array<PackedScene> resourceList;

	[Export] public Array<Currency> goods;

	void matchCurrency() {
		switch (type) {
			case NaturalResourceType.Food:
				resourceList = new Array<PackedScene> {
					GD.Load<PackedScene>("res://Scenes/Currency/Food.tscn")
				};
				break;
		}
	}

	public override void _Ready() { }

	void onHarvest(Character player) {
		for (int i = 0; i < resourceList.Count; i++) {
			Currency good = resourceList[i].Instantiate<Currency>();
			goods.Add(good);
			player.AddResource(goods[i]);
		}
	}
}