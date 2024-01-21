using Godot;
using GameLogick.Utilities;
using GameLogick.StateMachine;
using Game.Components;


namespace Game.Enteties
{


	public partial class Knigth : CharacterBody2D  , IEnemy
	{
		CharacterBody2D player;
		game_events Game_Events;
		[Export] HealthComponent healthComponent;
		[Export] EntitySpriteImager knigthSpriteImager;
		[Export] PathFindingComponent pathFindingComponent;
		[Export] VelocityComponent velocityComponent;
		[Export] WeaponRootComponent weaponRootComponent;
		[Export] AnimationPlayer animationPlayer;
		[Export] RayCast2D lineOfSigth;
		[Export] DeathSceneComponent deathSceneComponent;
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
			stateMachine.AddState(DeadState);
			stateMachine.AddState(NormalState , EnteredNormalState);
			stateMachine.AddState(AtackState  , EnterAtackState);
			stateMachine.AddState(DetectionState);
			stateMachine.SetInitiioalState(NormalState);
		}

		#endregion

		#region Connect to Signals
		private void ConnectToSginals()
		{
			healthComponent.Connect(HealthComponent.SignalName.Died , Callable.From(()=> stateMachine.ChangeState(DeadState)));
			pathFindingComponent.Connect(PathFindingComponent.SignalName.NavigationFinished , Callable.From(()=> animationPlayer.Stop()));	
		}
		#endregion
		public override void _Process(double delta)
		{
			if(!GameUtilities.CheckIfPlayerExist(this)){
				return;
			}
			
			stateMachine.Update();	
			knigthSpriteImager.LookAtTarget(player.Position);
			
		}
		

		private void EnteredNormalState()
		{
			GetTree().CreateTimer(1).Connect(Timer.SignalName.Timeout , Callable.From(()=>
				stateMachine.ChangeState(DetectionState)));
		}
		private void NormalState()
		{
			velocityComponent.Move(this);
			pathFindingComponent.FollowPath();
			pathFindingComponent.SetTargetPosition(player?.GlobalPosition ?? GlobalPosition);
			animationPlayer.Play("Walking");
		}
		private void EnterAtackState()
		{
			animationPlayer.Stop();
			GetTree().CreateTimer(2).Connect(Timer.SignalName.Timeout , Callable.From(()=> stateMachine.ChangeState(NormalState)));
		}
		private void AtackState()
		{
			var directionToShoot = (player.GlobalPosition - GlobalPosition).Normalized();
			weaponRootComponent.ShootFromCurrentWeapon(directionToShoot);
		}
		private  void DeadState()
		{
			Game_Events.EmitEnemyDeathSignal(Position , 10);
			QueueFree();
		}
		private void DetectionState()
		{
			
			if(lineOfSigth.IsColliding())
			{
				if(lineOfSigth.GetCollider() is HurtBoxComponent hurtBoxComponent)
				{
					stateMachine.ChangeState(AtackState);
				}
				else{
					
					stateMachine.ChangeState(NormalState);
					
				}
			}
			else{
				stateMachine.ChangeState(NormalState);
			}
		}

        public void OnWaveFinished()
        {
			deathSceneComponent.OnEnemyDied();
			QueueFree();
		}

    }
}

