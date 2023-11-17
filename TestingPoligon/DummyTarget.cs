using Godot;
using System;
using Game.Components;
using GameLogick.Utilities;
using GameLogick.StateMachine;
public partial class DummyTarget : CharacterBody2D
{
    [Export] HurtBoxComponent hurtBoxComponent;
    [Export] HealthComponent healthComponent;
    [Export] Area2D baffArea;
    [Export] PackedScene effect;
    CharacterBody2D player;
    
    public override void _Ready()
    {
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
    // fix bug when enemy cross multiple areas the baff is  going on 
}
