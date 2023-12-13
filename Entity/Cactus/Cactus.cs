using Godot;
using System;
using GameLogick.StateMachine;
using Game.Components;
using GameLogick.Utilities;
public partial class Cactus : CharacterBody2D
{
    DelegateStateMachine stateMachine = new DelegateStateMachine();
    [Export(PropertyHint.Range, "0,10, 0.2 * MathF.PI")] private float alpha = 0;
    [Export] PackedScene BulletScene;
    [Export] Timer atackTimer;
    HealthComponent healthComponent;
    private float theta = 0;
    public override void _Ready()
    {
        stateMachine.AddState(Dead);
        stateMachine.AddState(NormalState , EnteredNormalState);
        stateMachine.AddState(AtackState , EnteredAtackState  ,ExitAtackState);
        stateMachine.SetInitiioalState(NormalState);
        healthComponent = GetNode<HealthComponent>("HealthComponent");
        healthComponent.Connect(HealthComponent.SignalName.Died , Callable.From(()=>{
            stateMachine.ChangeState(Dead);         
        }));
        atackTimer.Connect(Timer.SignalName.Timeout, Callable.From(() =>
        {
            ShootBulletWithAngleOffset(theta);
        }));
    }
    public override void _Process(double delta)
    {
      if(!GameUtilities.CheckIfPlayerExist(this)){
			  return;
		  }
		
		stateMachine.Update();	
		}

    private Vector2 GetVector(float angle)
    {
        theta = angle + alpha;
        return new Vector2(MathF.Cos(theta), MathF.Sin(theta));
    }
    private void ShootBulletWithAngleOffset(float angle)
    {
        var bullet = BulletScene.Instantiate() as EnemyBullet;
        bullet.Position = GlobalPosition;
        bullet.direction = GetVector(angle);
        GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(bullet);
    }
   
    private void EnteredNormalState()
    {
      GetTree().CreateTimer(2).Connect(Timer.SignalName.Timeout , Callable.From(()=>{
        stateMachine.ChangeState(AtackState);
      }));
    }
    private void NormalState(){}
    private void EnteredAtackState()
	  {
        GetTree().CreateTimer(5).Connect(Timer.SignalName.Timeout , Callable.From(()=>{
        stateMachine.ChangeState(NormalState);
        }));
        atackTimer.Start(0.1f);
    }
    private void AtackState()
    {
    }
    private void ExitAtackState()
    {
      atackTimer.Stop();
    }
    
    private void Dead(){
      QueueFree();
    }

}
