using Godot;
using System;
using GameLogick.StateMachine;
using Game.Components;
using GameLogick.Utilities;
using Generation.Alghoritms;
public partial class Cactus : CharacterBody2D
{
    DelegateStateMachine stateMachine = new DelegateStateMachine();
    [Export(PropertyHint.Range , "0,10, 0.2 * MathF.PI")] private float alpha = 1.2f;
    [Export] PackedScene BulletScene;
    [Export] Timer atackTimer;
    [Export] AnimationPlayer animationPlayer;
    [Export] AudioStreamPlayer2D audioStreamPlayer;
    HealthComponent healthComponent;
    private float theta = 0;
    public override void _Ready()
    {
        alpha = Directions.random.Randfn(0f , 3f);
        
        healthComponent = GetNode<HealthComponent>("HealthComponent");

        InitializeStateMachine();
        ConnectToSginals();

    }
    #region Initialize State Machine 
  	private void InitializeStateMachine()
	  {
	    stateMachine.AddState(Dead);
      stateMachine.AddState(NormalState , EnteredNormalState);
      stateMachine.AddState(AtackState , EnteredAtackState , ExitAtackState);
      stateMachine.SetInitiioalState(NormalState);
	  }

	  #endregion

	  #region Connect to Signals
	  private void ConnectToSginals()
	  {
		    healthComponent.Connect(HealthComponent.SignalName.Died , Callable.From(()=>{
            stateMachine.ChangeState(Dead);         
        }));
        atackTimer.Connect(Timer.SignalName.Timeout, Callable.From(() =>
        {
            audioStreamPlayer.Play();
            ShootBulletWithAngleOffset(theta);
        }));
	  }
	#endregion
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
      animationPlayer.Play("idel");
      GetTree().CreateTimer(2).Connect(Timer.SignalName.Timeout , Callable.From(()=>{
        stateMachine.ChangeState(AtackState);
      }));
    }
    private void NormalState(){}
    private void EnteredAtackState()
	  {
        animationPlayer.Play("atack");
        GetTree().CreateTimer(5).Connect(Timer.SignalName.Timeout , Callable.From(()=>{
          stateMachine.ChangeState(NormalState);
        }));
        atackTimer.Start(0.13f);
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
