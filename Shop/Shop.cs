using Godot;
using System;

public partial class Shop : Node2D
{
   
    DialogeManager _dialogeManager;
    Marker2D _dialogBoxPosition;
    Area2D _interactionArea;
    game_events _gameEvents;
    PackedScene _appearanceParticle;
    private bool _canInteract = false;
    private readonly Godot.Collections.Array<string> _interactionMessages = new Godot.Collections.Array<string>
    {
        "Press I to interact and Enter to Advance Dialog",
        "Hi ('press Enter ->')",
        "Bro Press B and I will show you something",
        "Mrgl mrgl mrgl",
        "When you finished just press P",
    };
    public override void _Ready()
    {
        
        SetDependencies();
        LoadResourses();
        ConnectToSignals();
        EmitAppearenceParticle();
    }
    private void EmitAppearenceParticle()
    {
        var _appearanceParticleInstance = _appearanceParticle.Instantiate() as GpuParticles2D;
        _appearanceParticleInstance.GlobalPosition = GlobalPosition;
        GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(_appearanceParticleInstance);
    }
    private void SetDependencies()
    {

        _gameEvents = GetNode<game_events>("/root/GameEvents");
        _dialogeManager = GetNode<DialogeManager>("/root/DialogeManager");
        _dialogBoxPosition = GetNode<Marker2D>("DialogBoxPosition");
        _interactionArea = GetNode<Area2D>("Area2D");

    }
    private void LoadResourses()
    {
        _appearanceParticle = ResourceLoader.Load<PackedScene>("res://Visuals/Particles/AppearenceParticle.tscn");
    }
    private void ConnectToSignals()
    {
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
            _gameEvents.EmitShopOpening();
        }
        if(@event.IsActionPressed("next_wave"))
        {
            _dialogeManager.FinishDialog();
            EmitAppearenceParticle();
            _gameEvents.EmitNewWaveStarting();
            QueueFree(); 
        }
    }  
}
