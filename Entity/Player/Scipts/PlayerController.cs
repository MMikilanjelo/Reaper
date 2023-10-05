using Godot;
using System;
using Game.Components;
using GameLogick.StateMachine;
using System.Runtime.CompilerServices;

public partial class PlayerController : CharacterBody2D
{
	[Export] VelocityComponent velocityComponent;
	[Export] BulletHandlerComponent bulletHandlerComponent;
	[Export] HealthComponent healthComponent;
	[Export] Timer AtackDeleyTimer;
	[Export] AnimationPlayer animationPlayer;
	[Export] AnimationPlayer weaponAnimation;
	[Export] PlayerSpriteImager Visuals;
	private bool _HasAmmoRemaining = true;
	private float AtackDeley = 0.2f;
	private game_events game_Events;
	
	public float xMovent;
	public float yMovent;
	Vector2 directionToTarget;

	private DelegateStateMachine delegateStateMachine = new ();
	private int BulletMoveSpeed = 300;
	public override void _Ready()
	{
		
		delegateStateMachine.AddState(NormalState );
		delegateStateMachine.AddState(Shoot , EnterShootState);
		delegateStateMachine.SetInitiioalState(NormalState);
		delegateStateMachine.AddState(DeadState);		
		game_Events = GetNode<game_events>("/root/GameEvents");
		game_Events.Connect(game_events.SignalName.OnAbilityUpgradeAded , new Callable(this , nameof(OnAbilityUpgradeAded)));
	   	game_Events.Connect(game_events.SignalName.OnRunOutAmmo , Callable.From(()=> _HasAmmoRemaining =false));
	}

	public override void _PhysicsProcess(double delta)
	{	
		delegateStateMachine.Update();
		
	}
	public Vector2 GetMovementVector()
	{
		Vector2 movementVector = Vector2.Zero;
		xMovent = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		yMovent = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");
		movementVector = new Vector2(xMovent , yMovent);
		return movementVector;
	}
	private void EnterShootState()
	{
		weaponAnimation.Play("Shoot");
	}
	void Shoot()
	{
		if(AtackDeleyTimer.IsStopped())
		{
			game_Events.EmitPlayerShootSignal(1);
			directionToTarget = (GetGlobalMousePosition() - GlobalPosition).Normalized();
			bulletHandlerComponent.Shoot( this  , directionToTarget , Visuals.shootPositionParticleEmiter.GlobalPosition , BulletMoveSpeed);
			Visuals.EmitBulletShelsParticle();
			AtackDeleyTimer.Start(AtackDeley);
			
		}
		else{
			delegateStateMachine.ChangeState(NormalState);
		}	
	}

	void NormalState()
	{
		
		
		Vector2 direction = GetMovementVector().Normalized();
		velocityComponent.AccelerateInDirection(direction);
		velocityComponent.Move(this);
		if(direction == Vector2.Zero)
		{
			animationPlayer.Play("Idel");
		}
		else{
			animationPlayer.Play("Walk");
		}
		if(Input.IsActionPressed("Shoot"))
		{
			if(AtackDeleyTimer.IsStopped() && _HasAmmoRemaining)
			{
				delegateStateMachine.ChangeState(Shoot);
			}
			
		}
		if(!healthComponent._HasHealthRamaining){
			delegateStateMachine.ChangeState(DeadState);
		}
		velocityComponent.AccelerateInDirection(direction);
	}
	void DeadState()
	{
		QueueFree();
	}
	
	private void OnAbilityUpgradeAded(Upgrade addedUpgrade ,  Godot.Collections.Dictionary<string , Godot.Collections.Dictionary<Upgrade , int>> currentPlayerUpgrades)
	{
		
	}


}
