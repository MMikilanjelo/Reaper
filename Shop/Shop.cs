using Godot;
using System;

public partial class Shop : Node2D
{
    DialogeManager _dialogeManager;
    Marker2D _dialogBoxPosition;
    Area2D _interactionArea;
    PackedScene _shopScreenScene;
    ResourcePreloader _resourcePreloader;
    game_events _gameEvents;
    AnimationPlayer _animationPlayer;
    GpuParticles2D _appearanceParticle;
    Sprite2D _shopImage;
    private bool _canInteract = false;
    private Godot.Collections.Array<string> _interactionMessages = new Godot.Collections.Array<string>
    {
        "Press I to interact and Enter to Advance Dialog",
        "Hi ('press Enter ->')",
        "Bro Press B and I will show you something",
        "Mrgl mrgl mrgl",
        "When you finished just press P",
    };
    [Export] private Godot.Collections.Array<ShopSlotData> _itemsInShop = new Godot.Collections.Array<ShopSlotData>();
    public override void _Ready()
    {
        SetDependencies();
        LoadResourses();
        ConnectToSignals();
        _appearanceParticle.Emitting = true;
    }
    private void SetDependencies()
    {
        _gameEvents = GetNode<game_events>("/root/GameEvents");
        _dialogeManager = GetNode<DialogeManager>("/root/DialogeManager");
        _dialogBoxPosition = GetNode<Marker2D>("DialogBoxPosition");
        _interactionArea = GetNode<Area2D>("Area2D");
        _resourcePreloader = GetNode<ResourcePreloader>("ResourcePreloader");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _appearanceParticle = GetNode<GpuParticles2D>("GPUParticles2D");
        _shopImage = GetNode<Sprite2D>("Sprite2D");
    }
    private void LoadResourses()
    {
        _shopScreenScene = _resourcePreloader.GetResource("ShopScreen") as PackedScene;
    }
    private void ConnectToSignals()
    {
        _gameEvents.ShopSlotPurchased += (ShopSlotData _purchasedSlot) => SpawnPurchasedItem(_purchasedSlot);
        _interactionArea.BodyEntered += (Node2D _otherBody ) => 
        { 
            if (_otherBody is PlayerController)
            {
                _canInteract = true; 
                _dialogeManager.StartDialog(_dialogBoxPosition.GlobalPosition , _interactionMessages); 
            }
              
        };
        _interactionArea.BodyExited += (Node2D _otherBody ) => 
        {
            if (_otherBody is PlayerController) 
            {
                _canInteract = false;
            } 
        };
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        if(!_canInteract) return;
        if(@event.IsActionPressed("interect"))
        {
            _dialogeManager.StartDialog(_dialogBoxPosition.GlobalPosition , _interactionMessages);
        }
        if(@event.IsActionPressed("open"))
        {
            OpenShop();
        }
        if(@event.IsActionPressed("next_wave"))
        {
            OnNextWave();
            GetTree().CreateTimer(_appearanceParticle.Lifetime).Connect(Timer.SignalName.Timeout , Callable.From(()=>
            {
                _gameEvents.EmitNewWaveStarting();
                QueueFree(); 
            }));
        }
    }
    private void OnNextWave()
    {
        _appearanceParticle.Emitting = true;
        _shopImage.Visible = false;
        _canInteract = false;
        _interactionArea.Visible = false;
        _dialogeManager.FinishDialog();
    }
    private void OpenShop()
    {
        var _shopScreenInstance = _shopScreenScene.Instantiate() as ShopScreen;
        AddChild(_shopScreenInstance);
        _shopScreenInstance.SetUpSlotsInShop(_itemsInShop);
    }
    private void SpawnPurchasedItem(ShopSlotData _itemData)
    {
        
    }


}
