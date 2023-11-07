using Godot;
using System;
using Game.Components;
using GameLogick.Utilities;
public partial class DummyTarget : CharacterBody2D
{
    [Export] HurtBoxComponent hurtBoxComponent;
    [Export] PackedScene BloodParticle;
    CharacterBody2D player;
    
    public override void _Ready()
    {
        player = GameUtilities.GetPlayerNode(this);
        hurtBoxComponent.Connect(HurtBoxComponent.SignalName.HitByHitBox , Callable.From((HitBoxComponent hitBoxComponent)=>
        {
            SpawnBlood();
        }));
    }
    public void SpawnBlood()
    {
        GD.Print("Blood");
        var blood = BloodParticle.Instantiate() as GpuParticles2D;
        GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(blood);
        blood.GlobalPosition = GlobalPosition;
        blood.Rotation  = GlobalPosition.AngleToPoint(-player.GlobalPosition);
    }
}
