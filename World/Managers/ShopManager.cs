using Godot;
using System;
using System.Collections.Generic;

public partial class ShopManager : Node
{
    //TODO: handle all locgick related to shop here;
	PackedScene _shopScreenScene;
	private ResourcePreloader _resoursePreloader;
	private Godot.Collections.Dictionary<ShopSlotData , bool> _posibleItemsInShop = new();
	game_events _gameEvents;
    public override void _Ready()
    {
		SetDependencies();
		LoadResourses();
		ConnectToSignals();
    }
	private void SetDependencies()
	{
		
		_gameEvents = GetNode<game_events>("/root/GameEvents");
		_resoursePreloader = GetNode<ResourcePreloader>("ResourcePreloader");
		_shopScreenScene = ResourceLoader.Load("res://UI/ShopScreen.tscn") as PackedScene;
	}
	private void LoadResourses()
	{
		foreach(var _shopSlotResourseName in _resoursePreloader.GetResourceList())
        {
            _posibleItemsInShop.Add(_resoursePreloader.GetResource(_shopSlotResourseName) as ShopSlotData , true);
        }
	}
	private void ConnectToSignals()
	{
		_gameEvents.Connect(game_events.SignalName.ShopSlotPurchased , Callable.From((ShopSlotData _shopSlotData)=> RemoveSlot(_shopSlotData)));
		_gameEvents.Connect(game_events.SignalName.ShopOpened , Callable.From(()=> OpenShop()));
		_gameEvents.Connect(game_events.SignalName.NewWaveStarted , Callable.From(()=> ResetItemsAvailable()));
	}
	private void RemoveSlot(ShopSlotData _shopSlotToRemove)
	{
		if(!_posibleItemsInShop.ContainsKey(_shopSlotToRemove)) 
        {
            return;
        }
        _posibleItemsInShop[_shopSlotToRemove] = false;
        if(_shopSlotToRemove._isUnique)
        {
		    _posibleItemsInShop.Remove(_shopSlotToRemove);
        }
        
	}
	private void OpenShop()
    {
        var _shopScreenInstance = _shopScreenScene.Instantiate() as ShopScreen;
        AddChild(_shopScreenInstance);
        _shopScreenInstance.SetUpSlotsInShop(_posibleItemsInShop);
    }
	private void ResetItemsAvailable()
	{
		foreach(var _shopSLot in _posibleItemsInShop.Keys)
		{
			_posibleItemsInShop[_shopSLot] = true;
		}
	}
	
}
