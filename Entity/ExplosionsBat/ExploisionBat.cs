using Godot;
using System;
using Game.Components;
using GameLogick.StateMachine;
using Enemy.Parts;
using GameLogick.Utilities;
using DotEffects;
public partial class ExploisionBat : CharacterBody2D
{
	[Export] EnemySensorComponent enemySensorComponent;
	[Export] PathFindingComponent pathFindingComponent;
	[Export] VelocityComponent velocityComponent;
	[Export] HealthComponent healthComponent;
	
	[Export] PackedScene ExperiencePeal;
	[Export] HitBoxComponent hitBoxComponent;
	[Export] AnimationPlayer Animation;
	[Export] private PackedScene afex;

	private DelegateStateMachine stateMachine = new DelegateStateMachine();
	CharacterBody2D player;
	game_events Game_Events;
	
	public override void _Ready()
	{
		Game_Events = GetNode<game_events>("/root/GameEvents");
		hitBoxComponent.AddEffecToHit(afex);
		player = GameUtilities.GetPlayerNode(this);
		
		InitializeStateMachine();
		ConnectToSginals();
	}
	#region Initialize State Machine 
	private void InitializeStateMachine()
	{
	   	stateMachine.AddState(StateNormal  , EnteredStateNormal);	
		stateMachine.AddState(DeadState);
		stateMachine.AddState(ExplodeState , EnterExplodeState);
		stateMachine.SetInitiioalState(StateNormal);
	}

	#endregion

	#region Connect to Signals
	private void ConnectToSginals()
	{
		healthComponent.Connect(HealthComponent.SignalName.Died ,Callable.From(()=> stateMachine.ChangeState(DeadState)));
	}
	#endregion
	public override void _Process(double delta)
	{
		if(!GameUtilities.CheckIfPlayerExist(this)) return;
		stateMachine.Update();
	}
	
	private void StateNormal()
	{
		if(enemySensorComponent.isInRange)
		{
			stateMachine.ChangeState(ExplodeState);
		}
		Animation.Play("FlyingAnimation");
		velocityComponent.Move(this);
		pathFindingComponent.FollowPath();
		pathFindingComponent.SetTargetPosition(player?.GlobalPosition ?? GlobalPosition);
		
	}
	private void EnterExplodeState()
	{
		
		Animation.Connect(AnimationPlayer.SignalName.AnimationFinished , Callable.From((string animationName)=> {
		if(animationName == "ExplodeAnimation")
		{
		
			QueueFree();
		}
		}));
		healthComponent.canAcceptDamage = false;
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
		Game_Events.EmitEnemyDeathSignal(Position , 10);
		QueueFree();
	}

	
}
