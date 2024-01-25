using Godot;
using System;
using System.Diagnostics;

public partial class ShopSlotData : Resource
{
	public enum ItemType 
	{
		Weapon,
		Afex,
		Artifact,
	}
	[Export] public ItemType _itemType;
	[Export] public string _itemName;
	[Export] public string _itemDescription;
	[Export] public Texture2D _itemTexture;
	[Export] public float _itemCost;
	[Export] public PackedScene _itemScene;
	[Export] public bool _isUnique = false;
}
