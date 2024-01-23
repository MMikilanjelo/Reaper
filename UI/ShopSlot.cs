using Godot;
using System;

public partial class ShopSlot : PanelContainer
{
	[Export] Label _itemNameLabel;
	[Export] Label _descriptionItemLabel;
	[Export] Label _costLable;
	[Export] Button _selectButton;
	[Export] TextureRect _slotTextureRect;
	AnimationPlayer _animationPlayer;
	AnimationPlayer _hoverAnimationPlayer;
	private ShopSlotData _itemData;
	private game_events _gameEvents;
	private bool _disabled = false;
	public override void _Ready()
	{
		_gameEvents = GetNode<game_events>("/root/GameEvents");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_hoverAnimationPlayer = GetNode<AnimationPlayer>("HoverAnimationPlayer");
		_selectButton.Pressed += () => OnPressButton();
		Connect(SignalName.MouseEntered , Callable.From(()=>
		{
			if(_disabled)
			{
				return;
			}
			_hoverAnimationPlayer.Play("hover");
		}));
	} 
	public void SetShopSlot(ShopSlotData _itemData , int _currentCurrency)
	{
		this._itemData = _itemData;
		_itemNameLabel.Text = _itemData?._itemName ?? "";
		_descriptionItemLabel.Text = _itemData?._itemDescription ?? "";
		_slotTextureRect.Texture = _itemData?._itemTexture; 
		_costLable.Text = $"{_currentCurrency.ToString()}/{_itemData._itemCost.ToString()}";
		if(_currentCurrency < _itemData._itemCost) _selectButton.Disabled = true;
	}
	private void OnPressButton()
	{
		if(_disabled)
		{
			return;
		}
		SelectSlot();
	}
	public void PlayIn(double deley = 0)
	{
		Modulate = Colors.Transparent;
		GetTree().CreateTimer(deley).Connect(Timer.SignalName.Timeout , Callable.From(()=>
		{
			_animationPlayer.Play("in");
		}));
		
	}
	public void PlayDiscard()
	{
		_animationPlayer.Play("discard");
	}
	public void SelectSlot()
	{
		_disabled = true;
		_animationPlayer.Connect(AnimationPlayer.SignalName.AnimationFinished , Callable.From((string animation_name)=>
		{
			if(animation_name == "selected")
			{
				_gameEvents.EmitShopSlotPurchased(_itemData);
				QueueFree();
			}
		}));
		_animationPlayer.Play("selected");
		
	}
}
