using Godot;
using System;
using GameLogick.StateMachine;
using Game.Components;
using Enemy.Parts;
using Generation.Alghoritms;
using GameLogick.Utilities;
using System.Collections.Generic;
using System.Net;
using Game.Enteties;
public partial class CrystalSlime : CharacterBody2D , IEnemy
{
    [Export] EnemySensorComponent enemySensorComponent;
    [Export] PathFindingComponent pathFindingComponent;
    [Export] VelocityComponent velocityComponent;
    [Export] EnemyConstructor enemyConstructor;
    [Export] HealthComponent healthComponent;
    [Export] PackedScene ExperiencePeal;
    [Export] HitBoxComponent hitBoxComponent;
    [Export] AnimationPlayer Animation;
    [Export] Node2D visuals;
    [Export] DeathSceneComponent deathSceneComponent;
    CharacterBody2D player;
    game_events Game_Events;
    private DelegateStateMachine stateMachine = new DelegateStateMachine();
    public override void _Ready()
    {

        Game_Events = GetNode<game_events>("/root/GameEvents");
        player = GameUtilities.GetPlayerNode(this);
        ConnectToSginals();
        InitializeStateMachine();
        
    }
    #region Initialize State Machine 
	private void InitializeStateMachine()
	{
	    stateMachine.AddState(StateNormal, EnteredStateNormal);
        stateMachine.AddState(DeadState);
        stateMachine.AddState(CloseRangeAtackState, EnterCLoseRangeAtack);
        stateMachine.SetInitiioalState(StateNormal);
	}

	#endregion

	#region Connect to Signals
	private void ConnectToSginals()
	{
		healthComponent.Connect(HealthComponent.SignalName.Died, Callable.From(() => stateMachine.ChangeState(DeadState)));	
	}
	#endregion
    public override void _Process(double delta)
    {
        if (!GameUtilities.CheckIfPlayerExist(this))
        {
            return;
        }
        var directionToPlayer = (player.GlobalPosition - GlobalPosition).Normalized();
        if (directionToPlayer.X > 0)
        {
            visuals.Scale = new Vector2(-1, 1);
        }
        else
        {
            visuals.Scale = new Vector2(1, 1);
        }
        stateMachine.Update();
    }
    private void EnterCLoseRangeAtack()
    {

        Animation.Play("AtackAnimation");
    }
    private void CloseRangeAtackState()
    {
        if (!Animation.IsPlaying())
        {
            stateMachine.ChangeState(StateNormal);

        }
    }
    private void StateNormal()
    {
        if (enemySensorComponent.isInRange)
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
        Animation.Play("WalkAnimation");
    }
    private void DeadState()
    {
        Game_Events.EmitEnemyDeathSignal(Position, 10);
        QueueFree();
    }

    public void OnWaveFinished()
    {
       deathSceneComponent.OnEnemyDied();
	   QueueFree();
    }
}
