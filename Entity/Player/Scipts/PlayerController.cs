using Godot;
using Game.Components;
using GameLogick.StateMachine;
using GameUI;
using System.Collections.Generic;
using System.Linq;
public partial class PlayerController : CharacterBody2D , IVisitable
{
	public VelocityComponent _velocityComponent;
	public HealthComponent _healthComponent;
	public AnimationPlayer _animationPlayer;
	public WeaponRootComponent _weaponRootComponent;
	public EntitySpriteImager _playerSpriteImager;
	private List<IVisitable> _visitableNodes = new();
  	private PackedScene floatingTextScene;
	private game_events _gameEvents;
	private	AchievementEvents _achievementEvents;
	private DelegateStateMachine delegateStateMachine = new ();
	
	public override void _Ready()
	{
    	SetUpNodes();
		InitializeStateMachine();
		ConnectToSginals();
		GetVisitableNodes();
	}
	// private void BaseSetUp()
	// {
	// 	_weaponRootComponent.ChangeWeapon(ResourceLoader.Load<PackedScene>("res://Weapons/Bananarang/Bananarang.tscn"));	
	// }
	private void SetUpNodes()
	{
		floatingTextScene = ResourceLoader.Load("res://UI/FloatingText.tscn") as PackedScene;
		
		_gameEvents = GetNode<game_events>("/root/GameEvents");	
		_achievementEvents = GetNode<AchievementEvents>("/root/AchievementsEvents");
		_velocityComponent = GetNode<VelocityComponent>("VelocityComponent");
		_healthComponent = GetNode<HealthComponent>("HealthComponent");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_weaponRootComponent = GetNode<WeaponRootComponent>("Visuals/CanvasGroup/RotationPivot/WeaponRootComponent");
		_playerSpriteImager = GetNode<EntitySpriteImager>("Visuals");
		
		
	}
	private void GetVisitableNodes()
	{
		_visitableNodes = GetChildren().OfType<IVisitable>().ToList();
		_visitableNodes.Add(_weaponRootComponent);
	}
	#region Initialize State Machine
	private void InitializeStateMachine()
	{
		delegateStateMachine.AddState(NormalState );
		delegateStateMachine.SetInitiioalState(NormalState);
		delegateStateMachine.AddState(DeadState);
	}
	#endregion

	#region Connect to Signals

	private void ConnectToSginals()
	{
		_healthComponent.Connect(HealthComponent.SignalName.HealthChanged , Callable.From((HealthComponent.HealthUpdate healthUpdate)  => OnDmg()));
		_gameEvents.Connect(game_events.SignalName.ShopSlotPurchased , Callable.From((ShopSlotData _itemData)=>
		{
			if(_itemData._itemType != ShopSlotData.ItemType.Weapon) return;
			_weaponRootComponent.ChangeWeapon(_itemData._itemScene);
		}));
		_weaponRootComponent.Connect(WeaponRootComponent.SignalName.ShotedFromWeapon , Callable.From((Vector2 _direction)=>
		{
			_gameEvents.EmitPlayerShootSignal(1);
		}));
	}
	
	#endregion



	public override void _PhysicsProcess(double delta)
	{	
		delegateStateMachine.Update();
		_playerSpriteImager.LookAtTarget(GetGlobalMousePosition());
	}
	public Vector2 GetMovementVector()
	{
		
		float xMovent = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		float yMovent = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");
		Vector2 movementVector = new Vector2(xMovent , yMovent);
		return movementVector;
	}



	void NormalState()
	{
		Vector2 direction = GetMovementVector().Normalized();
		_velocityComponent.AccelerateInDirection(direction);
		_velocityComponent.Move(this);
		if(direction == Vector2.Zero)
		{
			_animationPlayer.Play("Idel");
		}
		else{
			_animationPlayer.Play("Walk");
		}
		if(Input.IsActionJustPressed("Shoot"))
		{
			var directionToShoot = (GetGlobalMousePosition() - GlobalPosition).Normalized();
			_weaponRootComponent.ShootFromCurrentWeapon(directionToShoot);
		}
		if(!_healthComponent._HasHealthRamaining){
			delegateStateMachine.ChangeState(DeadState);
		}
		_velocityComponent.AccelerateInDirection(direction);
	}
	
	void DeadState()
	{
		_achievementEvents.EmitAchievementUnlocked("crab");
		QueueFree();
	}
	private void OnDmg()
	{ 
		// var Missed_text = floatingTextScene.Instantiate() as FloatingText;
		// Missed_text.SetScale(0.5f);
		// GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(Missed_text);
		// Missed_text.GlobalPosition = GlobalPosition + Vector2.Right;
		// Missed_text.Start("@#*!$!");
	}

    public void Accept(IVisitor visitor)
    {
        foreach(var _visitable in _visitableNodes)
		{
			_visitable.Accept(visitor);
		}
    }

}
