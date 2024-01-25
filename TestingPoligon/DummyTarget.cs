using Godot;
using System;
using Game.Components;
using GameLogick.Utilities;
using GameLogick.StateMachine;
using Game.Enteties;
public partial class DummyTarget : CharacterBody2D , IEnemy
{
    [Export] HurtBoxComponent hurtBoxComponent;
    [Export] HealthComponent healthComponent;
    [Export] Area2D baffArea;
    [Export] PackedScene effect;
    [Export] DeathSceneComponent deathSceneComponent;
    CharacterBody2D player;
    game_events _gameEvents;
    public override void _Ready()
    {
        _gameEvents = GetNode<game_events>("/root/GameEvents");
        baffArea.Connect(Area2D.SignalName.BodyEntered , Callable.From((Node2D otherBody)=>
        {
            if(otherBody is not DummyTarget)
            {
                ApplyBaff(otherBody);   
            }
            
        }));
        
        baffArea.Connect(Area2D.SignalName.BodyExited , Callable.From((Node2D otherBody)=> RemoveBaff(otherBody)));
        healthComponent.Connect(HealthComponent.SignalName.Died , Callable.From(()=>
        {
            QueueFree();
        }));

        _gameEvents.Connect(game_events.SignalName.WaveFinished , Callable.From(()=>
		{
			OnWaveFinished();
		}
		));
        
        
        player = GameUtilities.GetPlayerNode(this);
       
    }
    public void ApplyBaff(Node2D otherBody)
    {
       StatusRecivierComponent entityStatusComponent = otherBody.GetNodeOrNull<StatusRecivierComponent>("StatusRecivierComponent");
       if(entityStatusComponent != null)
       {
            entityStatusComponent.ApplyEffect(effect);
       }
      
    }
    public void RemoveBaff(Node2D otherBody)
    {
        StatusRecivierComponent entityStatusComponent = otherBody.GetNodeOrNull<StatusRecivierComponent>("StatusRecivierComponent");
        if(entityStatusComponent != null)
        {
            entityStatusComponent.ForcedRemoveEffect();
        }
        
    }
    public override void _ExitTree()
    {
        _gameEvents.EmitEnemyDeathSignal(Position , 1);
    }
    public void OnWaveFinished()
    {
        deathSceneComponent.OnEnemyDied();
		QueueFree();
    } 

}
