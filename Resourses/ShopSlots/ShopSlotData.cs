using Godot;
using System;

public partial class ShopSlotData : Node
{
	[Export] public string _itemName;
	[Export] public string _itemDescription;
	[Export] public Texture2D _itemTexture;
	[Export] public float _itemCost;
}
