using Godot;
using System;
using Game.Components;
using GameLogick.Utilities;
using GameLogick.StateMachine;
public partial class DummyTarget : CharacterBody2D
{
    [Export] HurtBoxComponent hurtBoxComponent;
    [Export] PackedScene enemyScene;
    [Export] Area2D baffArea;
    [Export] PackedScene effect;
    CharacterBody2D player;
    
    public override void _Ready()
    {
        baffArea.Connect(Area2D.SignalName.BodyEntered , Callable.From((Node2D otherBody)=>
        ApplyBaff(otherBody)));
         baffArea.Connect(Area2D.SignalName.BodyExited , Callable.From((Node2D otherBody)=>
        RemoveBaff(otherBody)));
        player = GameUtilities.GetPlayerNode(this);
        
    }
    
    public void ApplyBaff(Node2D otherBody)
    {
       StatusRecivierComponent entityStatusComponent = otherBody.GetNodeOrNull<StatusRecivierComponent>("StatusRecivierComponent");
       if(entityStatusComponent == null)
       {
            return;
       }
       entityStatusComponent.ApplyEffect(effect);
    }
    public void RemoveBaff(Node2D otherBody)
    {
        StatusRecivierComponent entityStatusComponent = otherBody.GetNodeOrNull<StatusRecivierComponent>("StatusRecivierComponent");
        if(entityStatusComponent == null)
        {
            return;
        }
        entityStatusComponent.ForcedRemoveEffect();
    }
}
