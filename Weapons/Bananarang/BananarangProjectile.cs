using Game.Components;
using Game.Weapons;
using GameLogick.StateMachine;
using Godot;
using System;
using GameLogick.Utilities;
public partial class BananarangProjectile : CharacterBody2D
{
	[Signal] public delegate void HitTheWallEventHandler();
	[Signal] public delegate void TurnedBackEventHandler();
	[Export] private float _timeBeforeTurnBack = 2f;
	VelocityComponent _velocityComponent;
	HitBoxComponent _hitBoxComponent;
	CollisionShape2D _collisionShape;
	Vector2 _direction;
	PlayerController _player;
	private float _maxFlightDistance = 100f;
	private DelegateStateMachine _stateMachine = new DelegateStateMachine();
    public override void _Ready()
    {
		
		SetDependencies();
		SetUpStateMachine();
		ConnectToSignals();
	}
	
	private void SetDependencies()
	{
		
		_hitBoxComponent = GetNode<HitBoxComponent>("HitBoxComponent");
		_velocityComponent = GetNode<VelocityComponent>("VelocityComponent");
		_player = GetTree().GetFirstNodeInGroup("Player") as PlayerController;
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
	}
	private void ConnectToSignals()
	{
		GetTree().CreateTimer(_timeBeforeTurnBack).Connect(Timer.SignalName.Timeout , Callable.From(()=>
		{
			if(_stateMachine.GetCurrentState != TurnBackState)
			{
				_stateMachine.ChangeState(TurnBackState);
			}
		}));
		_hitBoxComponent.Connect(HitBoxComponent.SignalName.OnWallCollide , new Callable(this, nameof(OnWallColide)));
	}
	public override void _PhysicsProcess(double delta)
    {
		if(!GameUtilities.CheckIfPlayerExist(this)){
			return;
		}
		_stateMachine.Update();
		_velocityComponent.Move(this);
	}
	private void SetUpStateMachine()
	{
		_stateMachine.AddState(NormalState);
		_stateMachine.AddState(TurnBackState ,EnterTurnBackState );
		_stateMachine.SetInitiioalState(NormalState);
	}
	private void NormalState()
	{
		_velocityComponent.AccelerateInDirection(_direction);
		if(GlobalPosition.DistanceTo(_player.GlobalPosition) > _maxFlightDistance )
		{
			_stateMachine.ChangeState(TurnBackState);
		}
	}
	private void EnterTurnBackState()
	{
		_collisionShape.Disabled = true;
	}
	private void TurnBackState()
	{

		var _newDirection = (_player.GlobalPosition - GlobalPosition).Normalized();
		_velocityComponent.AccelerateInDirection(_newDirection);
		if(GlobalPosition.DistanceTo(_player.GlobalPosition) < 2f)
		{
			EmitSignal(SignalName.TurnedBack);
			QueueFree();	
		}
	}
	public void ApplyAfexForProjectile(PackedScene afex)
	{
		if(afex != null)
		{
			_hitBoxComponent.AddEffecToHit(afex);
		}
	}
	private void OnWallColide()
	{
		_stateMachine.ChangeState(TurnBackState);
		EmitSignal(SignalName.HitTheWall);
	}
	public void SetProjectileDirection(Vector2 _direction)
	{
		this._direction = _direction;
	}

}
