using Godot;
using System;
using Game.Components;
using GameLogick.StateMachine;
using System.Runtime.CompilerServices;

public partial class PlayerController : CharacterBody2D
{
	[Export]public  VelocityComponent velocityComponent;
	[Export]public  HealthComponent healthComponent;
	[Export]  AnimationPlayer animationPlayer;
	[Export] WeaponRootComponent weaponRootComponent;
	[Export] EntitySpriteImager playerSpriteImager;
	private game_events game_Events;
	
	public float xMovent;
	public float yMovent;

	private DelegateStateMachine delegateStateMachine = new ();

	public override void _Ready()
	{
		
		delegateStateMachine.AddState(NormalState );
		delegateStateMachine.SetInitiioalState(NormalState);
		delegateStateMachine.AddState(DeadState);		
		game_Events = GetNode<game_events>("/root/GameEvents");	
		weaponRootComponent.Connect(WeaponRootComponent.SignalName.ShotedFromWeapon , Callable.From(()=>{
			playerSpriteImager.EmitBulletShelsParticle();
			game_Events.EmitPlayerShootSignal(1);
		}));
	}

	public override void _PhysicsProcess(double delta)
	{	
		delegateStateMachine.Update();
		playerSpriteImager.LookAtTarget(GetGlobalMousePosition());
		
	}
	public Vector2 GetMovementVector()
	{
		Vector2 movementVector = Vector2.Zero;
		xMovent = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		yMovent = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");
		movementVector = new Vector2(xMovent , yMovent);
		return movementVector;
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
			
			var directionToShoot = (GetGlobalMousePosition() - GlobalPosition).Normalized();
			weaponRootComponent.ShootFromCurrentWeapon(directionToShoot);
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

	


}
