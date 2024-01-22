using Godot;
using System;

public partial class ShopScreen : CanvasLayer
{
	private ResourcePreloader _resourcePreloader;
	private PackedScene _slotCardContainerScene;
    public override void _Ready()
    {
		_resourcePreloader = GetNode<ResourcePreloader>("ResourcePreloader");
		_slotCardContainerScene =  _resourcePreloader.GetResource("ShopSlot")  as PackedScene;
	}
	public void SetUpSlotsInShop(Godot.Collections.Array<ShopSlotData> _possibleSlots)
	{
		foreach (var _slotData in _possibleSlots)
		{
			var _slotCardContainer = _slotCardContainerScene.Instantiate() as ShopSlot;
			_slotCardContainer.SetShopSlot(_slotData);
		}
	}
}
