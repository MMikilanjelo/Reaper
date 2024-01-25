using Game.Components;
using Godot;
using System;

public partial class PoisonArtifact : Node2D, IVisitor
{

    public void Visit(HealthComponent _healthComponent){}

    public void Visit(HurtBoxComponent _hurtBoxComponent) {}

    public void Visit(VelocityComponent _velocityComponent){}

    public void Visit(WeaponRootComponent _weaponRootComponent)
    {
        _weaponRootComponent.AddAfexToWeapon(ResourceLoader.Load<PackedScene>("res://DotEffects/ToxicEffect/ToxicDotEffect.tscn"));
    }
}
