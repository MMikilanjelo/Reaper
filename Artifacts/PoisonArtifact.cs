using Game.Components;
using Godot;
using System;

public partial class PoisonArtifact : Node2D, IVisitor
{
    private PackedScene _poisonEffectScene = ResourceLoader.Load<PackedScene>("res://DotEffects/ToxicEffect/ToxicDotEffect.tscn");
    private game_events _gameEvents;
    public override void _Ready()
    {
        _gameEvents = GetNode<game_events>("/root/GameEvents");
        _gameEvents.Connect(game_events.SignalName.OnEnemyDmgRecived , Callable.From((int _dmgRecieved , EnemyData _hittedEnemy)=>
        {
            
            _hittedEnemy._statusRecivierComponent.ApplyEffect(_poisonEffectScene);
        }));
    }
    public void Visit(HealthComponent _healthComponent){}

    public void Visit(HurtBoxComponent _hurtBoxComponent) {}

    public void Visit(VelocityComponent _velocityComponent){}

    public void Visit(WeaponRootComponent _weaponRootComponent)
    {
        //_weaponRootComponent.AddAfexToWeapon(ResourceLoader.Load<PackedScene>("res://DotEffects/ToxicEffect/ToxicDotEffect.tscn"));
    }
    public void Visit(StatusRecivierComponent _statusRecivierComponent){}
}
