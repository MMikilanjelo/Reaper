using Godot;
using System;
using Game.Components;
using GameLogick.StateMachine;
using Enemy.Parts;
using GameLogick.Utilities;
public partial class ExploisionBat : CharacterBody2D
{
	[Export] EnemySensorComponent enemySensorComponent;
	[Export] PathFindingComponent pathFindingComponent;
	[Export] VelocityComponent velocityComponent;
	[Export] HealthComponent healthComponent;
	
	[Export] PackedScene ExperiencePeal;
	[Export] HitBoxComponent hitBoxComponent;
	[Export] AnimationPlayer Animation;
	private DelegateStateMachine stateMachine = new DelegateStateMachine();
	CharacterBody2D player;
	public override void _Ready()
	{
		player = GameUtilities.GetPlayerNode(this);
		enemySensorComponent.Connect(EnemySensorComponent.SignalName.PlayerEnteredInRange ,Callable.From(()=>
		stateMachine.ChangeState(ExplodeState)));
		healthComponent.Connect(HealthComponent.SignalName.Died ,Callable.From(()=> stateMachine.ChangeState(DeadState)));
		stateMachine.AddState(StateNormal  , EnteredStateNormal);	
		stateMachine.AddState(DeadState);
		stateMachine.AddState(ExplodeState , EnterExplodeState);
		stateMachine.SetInitiioalState(StateNormal);
	}
	public override void _Process(double delta)
	{
		if(!GameUtilities.CheckIfPlayerExist(this)) return;
		stateMachine.Update();
	}
	
	private void StateNormal()
	{
		
		Animation.Play("FlyingAnimation");
		velocityComponent.Move(this);
		pathFindingComponent.FollowPath();
		pathFindingComponent.SetTargetPosition(player?.GlobalPosition ?? GlobalPosition);
	}
	private void EnterExplodeState()
	{
		healthComponent.canAcceptDamage = false;
		Animation.Connect(AnimationPlayer.SignalName.AnimationFinished , Callable.From((string animationName)=> {
		if(animationName == "ExplodeAnimation")
		{
				QueueFree();
		}
			
			
		}));
		Animation.Play("ExplodeAnimation");
		
	}
	private void ExplodeState()
	{
		
	}
	private void EnteredStateNormal()
	{	
		pathFindingComponent.SetTargetPosition(player?.GlobalPosition ?? GlobalPosition ); 
	}
	
	
	private  void DeadState()
	{
		experience exp = ExperiencePeal.Instantiate() as experience;
		exp.Position = this.GlobalPosition;
		GetTree().GetFirstNodeInGroup("EntitiesLayer").AddChild(exp);
		QueueFree();
	}

	
}
