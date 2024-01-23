using Godot;
using System;

public partial class ShopScreen : CanvasLayer
{
	private ResourcePreloader _resourcePreloader;
	private PackedScene _slotCardContainerScene;
	private UIEvents _uiEvents;
	private GridContainer _shopSlotsContainer;
    public override void _Ready()
    {
		_shopSlotsContainer = GetNode<GridContainer>("Control/OuterMarginContainer/PanelContainer/ScrollContainer/InnerMarginContainer/GridContainer");
		_uiEvents = GetNode<UIEvents>("/root/UIEvents");
		_resourcePreloader = GetNode<ResourcePreloader>("ResourcePreloader");
		_slotCardContainerScene =  _resourcePreloader.GetResource("ShopSlot")  as PackedScene;
		GetTree().Paused = true;
	}
	public void SetUpSlotsInShop(Godot.Collections.Array<ShopSlotData> _possibleSlots)
	{
		foreach (var _slotData in _possibleSlots)
		{
			var _slotCardContainer = _slotCardContainerScene.Instantiate() as ShopSlot;
			_shopSlotsContainer.AddChild(_slotCardContainer);
			_slotCardContainer.SetShopSlot(_slotData , _uiEvents.playerStatistic._currentCurrency);
		}
	}
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("CloseTab"))
		{
			GetTree().Paused = false;
			QueueFree();
		}
    }
}
