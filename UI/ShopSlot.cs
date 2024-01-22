using Godot;
using System;

public partial class ShopSlot : PanelContainer
{
	[Export] Label _itemNameLabel;
	[Export] Label _descriptionItemLabel;
	[Export] Button _selectButton;
	AnimationPlayer _animationPlayer;
	AnimationPlayer _hoverAnimationPlayer;
	[Signal] public delegate void ShopSlotSelectedEventHandler();
	private bool _disabled = false;
	public override void _Ready()
	{
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
	public void SetShopSlot(ShopSlotData _itemData)
	{
		_itemNameLabel.Text = _itemData?._itemName ?? "";
		_descriptionItemLabel.Text = _itemData?._itemDescription ?? "";
	}
	private void OnPressButton()
	{
		if(_disabled)
		{
			return;
		}
		SelectCard();
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
	public void SelectCard()
	{
		_disabled = true;
		_animationPlayer.Connect(AnimationPlayer.SignalName.AnimationFinished , Callable.From((string animation_name)=>
		{
			if(animation_name == "selected")
			{
				EmitSignal(SignalName.ShopSlotSelected);
				QueueFree();
			}
		}));
		_animationPlayer.Play("selected");
		
	}
}
