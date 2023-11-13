using Godot;
using System;
using GameLogick.StateMachine;
using Game.Components;
using Enemy.Parts;
using Generation.Alghoritms;
using GameLogick.Utilities;
using System.Collections.Generic;
using System.Net;
public partial class CrystalSlime : CharacterBody2D
{
	[Export] EnemySensorComponent enemySensorComponent;
	[Export] PathFindingComponent pathFindingComponent;
	[Export] VelocityComponent velocityComponent;
	[Export] EnemyConstructor enemyConstructor;
	[Export] HealthComponent healthComponent;
	[Export] PackedScene ExperiencePeal;
	[Export] HitBoxComponent hitBoxComponent;
	[Export] AnimationPlayer Animation;
	CharacterBody2D player;
	game_events Game_Events;
	private DelegateStateMachine stateMachine = new DelegateStateMachine();
	public override void _Ready()
	{
	
		Game_Events = GetNode<game_events>("/root/GameEvents");
		healthComponent.Connect(HealthComponent.SignalName.Died , Callable.From(()=> stateMachine.ChangeState(DeadState)));
		player = GameUtilities.GetPlayerNode(this);
		stateMachine.AddState(StateNormal , EnteredStateNormal );
		stateMachine.AddState(DeadState);
		stateMachine.AddState( CloseRangeAtackState , EnterCLoseRangeAtack);	
		stateMachine.SetInitiioalState(StateNormal);
	}
	public override void _Process(double delta)
	{
		if(!GameUtilities.CheckIfPlayerExist(this)){
			return;
		}
	
		stateMachine.Update();
	}
	private void EnterCLoseRangeAtack()
	{
		
		Animation.Play("AtackAnimation");
		
	}
	private void CloseRangeAtackState()
	{
		if(!Animation.IsPlaying()){
			stateMachine.ChangeState(StateNormal);
			
		}
	}
	private void StateNormal()
	{
		if(enemySensorComponent.isInRange)
		{
			stateMachine.ChangeState(CloseRangeAtackState);
		}
		velocityComponent.Move(this);
		pathFindingComponent.FollowPath();
		pathFindingComponent.SetTargetPosition(player?.GlobalPosition ?? GlobalPosition); 
	}
	
	private void EnteredStateNormal()
	{
		pathFindingComponent.SetTargetPosition(player?.GlobalPosition ?? GlobalPosition);
  	}
	private  void DeadState()
	{
		
		Game_Events.EmitEnemyDeathSignal(Position , 10);
		QueueFree();
	}
	
	
}
