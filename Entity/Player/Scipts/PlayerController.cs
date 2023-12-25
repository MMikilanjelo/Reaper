using Godot;
using Game.Components;
using GameLogick.StateMachine;
public partial class PlayerController : CharacterBody2D
{
	[Export]public  VelocityComponent velocityComponent;
	[Export]public  HealthComponent healthComponent;
	[Export]  AnimationPlayer animationPlayer;
	[Export] WeaponRootComponent weaponRootComponent;
	[Export] EntitySpriteImager playerSpriteImager;
  	private PackedScene floatingTextScene;
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
    	floatingTextScene = ResourceLoader.Load("res://UI/FloatingText.tscn") as PackedScene;
    	healthComponent.Connect(HealthComponent.SignalName.HealthChanged , Callable.From((HealthComponent.HealthUpdate healthUpdate)
      	=> OnDmg()));
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
  private void OnDmg()
  { 
    // var Missed_text = floatingTextScene.Instantiate() as FloatingText;
    // Missed_text.SetScale(0.5f);
	//	GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(Missed_text);
	//	Missed_text.GlobalPosition = GlobalPosition + Vector2.Right;
	// Missed_text.Start("@#*!$!");
  }
}
