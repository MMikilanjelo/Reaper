using Godot;
using System;
using GameLogick.Utilities;
using GameLogick.StateMachine;
using Game.Components;
	

public partial class Knigth : CharacterBody2D
{
	CharacterBody2D player;
	game_events Game_Events;
	[Export] HealthComponent healthComponent;
	[Export] EntitySpriteImager knigthSpriteImager;
	[Export] PathFindingComponent pathFindingComponent;
	[Export] VelocityComponent velocityComponent;
	[Export] WeaponRootComponent weaponRootComponent;
	[Export] AnimationPlayer animationPlayer;
	private DelegateStateMachine stateMachine = new DelegateStateMachine();
    public override void _Ready()
    {
       	Game_Events = GetNode<game_events>("/root/GameEvents");
		healthComponent.Connect(HealthComponent.SignalName.Died , Callable.From(()=> stateMachine.ChangeState(DeadState)));
		pathFindingComponent.Connect(PathFindingComponent.SignalName.NavigationFinished , Callable.From(()=>
		{	
			animationPlayer.Stop();
		}));
		
		player = GameUtilities.GetPlayerNode(this);
		stateMachine.AddState(DeadState);
		stateMachine.AddState(NormalState , EnteredNormalState);
		stateMachine.AddState(AtackState  , EnterAtackState);
		stateMachine.SetInitiioalState(NormalState);
    }
    public override void _Process(double delta)
    {
        if(!GameUtilities.CheckIfPlayerExist(this)){
			return;
		}
		
		stateMachine.Update();
		knigthSpriteImager.LookAtTarget(player.Position);
    }
	
   
	private void EnteredNormalState()
	{
		GetTree().CreateTimer(5).Connect(Timer.SignalName.Timeout , Callable.From(()=> stateMachine.ChangeState(AtackState)));
  	}
	private void NormalState()
	{
		
		velocityComponent.Move(this);
		pathFindingComponent.FollowPath();
		pathFindingComponent.SetTargetPosition(player?.GlobalPosition ?? GlobalPosition);
		animationPlayer.Play("Walking");
		
		
	}
	private void EnterAtackState()
	{
		GetTree().CreateTimer(3).Connect(Timer.SignalName.Timeout , Callable.From(()=> stateMachine.ChangeState(NormalState)));
	}
	private void AtackState()
	{
		var directionToShoot = (player.GlobalPosition - GlobalPosition).Normalized();
		weaponRootComponent.ShootFromCurrentWeapon(directionToShoot);
	}
	 private  void DeadState()
	{
		Game_Events.EmitEnemyDeathSignal(Position , 10);
		QueueFree();
	}
	
}
