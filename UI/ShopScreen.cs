using Godot;
using System;
using System.Collections.Generic;

public partial class ShopScreen : CanvasLayer
{
	private ResourcePreloader _resourcePreloader;
	private PackedScene _slotCardContainerScene;
	private UIEvents _uiEvents;
	private game_events _gameEvents;
	private GridContainer _shopSlotsContainer;
    public override void _Ready()
    {
		_gameEvents = GetNode<game_events>("/root/GameEvents");
		_shopSlotsContainer = GetNode<GridContainer>("Control/OuterMarginContainer/PanelContainer/ScrollContainer/InnerMarginContainer/GridContainer");
		_uiEvents = GetNode<UIEvents>("/root/UIEvents");
		_resourcePreloader = GetNode<ResourcePreloader>("ResourcePreloader");
		_slotCardContainerScene =  _resourcePreloader.GetResource("ShopSlot")  as PackedScene;
		
		GetTree().Paused = true;
	}
	public void SetUpSlotsInShop(Godot.Collections.Dictionary<ShopSlotData , bool>  _possibleSlots)
	{
		foreach (var kvp in _possibleSlots)
		{
			var slotData = kvp.Key;
			var isActive = kvp.Value;

			if (!isActive)
				continue;

			var slotCardContainer = _slotCardContainerScene.Instantiate() as ShopSlot;
			_shopSlotsContainer.AddChild(slotCardContainer);
			slotCardContainer.SetShopSlot(slotData, _uiEvents.playerStatistic._currentCurrency);
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
