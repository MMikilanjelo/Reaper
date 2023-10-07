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

	private DelegateStateMachine stateMachine = new DelegateStateMachine();
	CharacterBody2D player;
	game_events Game_Events;
	public override void _Ready()
	{
		Game_Events = GetNode<game_events>("/root/GameEvents");
		player = GameUtilities.GetPlayerNode(this);
		healthComponent.Connect(HealthComponent.SignalName.Died ,Callable.From(()=> stateMachine.ChangeState(DeadState)));
		hitBoxComponent.effect = ResourceLoader.Load<PackedScene>("res://DotEffects/ToxicDotEffect.tscn");	
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
