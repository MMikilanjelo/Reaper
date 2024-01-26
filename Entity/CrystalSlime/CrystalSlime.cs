using Godot;
using System;
using GameLogick.StateMachine;
using Game.Components;
using GameLogick.Utilities;

namespace Game.Enteties
{


public partial class CrystalSlime : CharacterBody2D , IEnemy
{
    private EnemySensorComponent _enemySensorComponent;
    private PathFindingComponent _pathFindingComponent;
    private VelocityComponent _velocityComponent;
    private HealthComponent _healthComponent;
    private HitBoxComponent _hitBoxComponent;
    private AnimationPlayer _animationPlayer;
    private Node2D _visuals;
    private DeathSceneComponent _deathSceneComponent;
    private CharacterBody2D _player;
    private game_events _gameEvents;
    private DelegateStateMachine _stateMachine = new DelegateStateMachine();
    public override void _Ready()
    {
        SetDependencies();
        ConnectToSginals();
        InitializeStateMachine();
        
    }
    #region Initialize State Machine 
	private void InitializeStateMachine()
	{
	    _stateMachine.AddState(StateNormal, EnteredStateNormal);
        _stateMachine.AddState(DeadState);
        _stateMachine.AddState(CloseRangeAtackState, EnterCLoseRangeAtack);
        _stateMachine.SetInitiioalState(StateNormal);

	}

	#endregion

	#region Connect to Signals
	private void ConnectToSginals()
	{
		_healthComponent.Connect(HealthComponent.SignalName.Died, Callable.From(() => _stateMachine.ChangeState(DeadState)));	
        _gameEvents.Connect(game_events.SignalName.WaveFinished , Callable.From(()=>
		{
			OnWaveFinished();
		}
		));
	}
	#endregion
    private void SetDependencies()
    {
        
        _gameEvents = GetNode<game_events>("/root/GameEvents");
        _player = GameUtilities.GetPlayerNode(this);
        _enemySensorComponent = GetNode<EnemySensorComponent>("Areas/EnemySensorComponent");
        _pathFindingComponent = GetNode<PathFindingComponent>("PathFindingComponent");
        _velocityComponent = GetNode<VelocityComponent>("VelocityComponent");
        _healthComponent = GetNode<HealthComponent>("HealthComponent");
        _hitBoxComponent = GetNode<HitBoxComponent>("Areas/HitBoxComponent");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _visuals = GetNode<Node2D>("Visuals");
        _deathSceneComponent = GetNode<DeathSceneComponent>("DeathSceneComponent");

    }
    public override void _Process(double delta)
    {
        if (!GameUtilities.CheckIfPlayerExist(this))
        {
            return;
        }
        var directionToPlayer = (_player.GlobalPosition - GlobalPosition).Normalized();
        if (directionToPlayer.X > 0)
        {
            _visuals.Scale = new Vector2(-1, 1);
        }
        else
        {
            _visuals.Scale = new Vector2(1, 1);
        }
        _stateMachine.Update();
    }
    private void EnterCLoseRangeAtack()
    {

        _animationPlayer.Play("AtackAnimation");
    }
    private void CloseRangeAtackState()
    {
        if (!_animationPlayer.IsPlaying())
        {
            _stateMachine.ChangeState(StateNormal);

        }
    }
    private void StateNormal()
    {
        if (_enemySensorComponent.isInRange)
        {
            _stateMachine.ChangeState(CloseRangeAtackState);
        }
        _velocityComponent.Move(this);
        _pathFindingComponent.FollowPath();
        _pathFindingComponent.SetTargetPosition(_player?.GlobalPosition ?? GlobalPosition);
    }

    private void EnteredStateNormal()
    {
        _pathFindingComponent.SetTargetPosition(_player?.GlobalPosition ?? GlobalPosition);
        _animationPlayer.Play("WalkAnimation");
    }
    private void DeadState()
    {
        _gameEvents.EmitEnemyDeathSignal(Position, 10);
        QueueFree();
    }

    public void OnWaveFinished()
    {
       _deathSceneComponent.OnEnemyDied();
	   QueueFree();
    }
}
}