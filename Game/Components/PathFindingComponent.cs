using Godot;
using System;
using Generation.Alghoritms;
using GameLogick.Utilities;
namespace Game.Components
{
	
	public partial class PathFindingComponent : Node2D
	{
		[Export] public  NavigationAgent2D NavigationAgent2D {get ;private set;}
		[Export] private VelocityComponent velocityComponent;
		[Export] public Timer intervalTimer;
		[Signal] public delegate void NavigationFinishedEventHandler();
		[Export] private bool MakeSmoothHalfWayDestination;
		[Export] private float ChangeNavigationInterval = 2f;
		CharacterBody2D player;
		public override void _Ready()
		{
			player = GameUtilities.GetPlayerNode(this);
			if(MakeSmoothHalfWayDestination)
			{
				NavigationAgent2D.Connect(NavigationAgent2D.SignalName.VelocityComputed , new Callable(this , nameof(OnVelocityComputed)));
			}
			
			
			intervalTimer.Connect(Timer.SignalName.Timeout ,  Callable.From( ()=>
			{
				if(!GameUtilities.CheckIfPlayerExist(this)){return ;}
				SetTargetPosition(player?.Position ?? Position);
			}
			));
		}
		
		public void SetTargetPosition(Vector2 targetPosition)
		{
			if(player != null)
			{
				if(!intervalTimer.IsStopped()){return;}
				intervalTimer.Start(Directions.random.Randfn(0.2f , ChangeNavigationInterval));
				NavigationAgent2D.TargetPosition = targetPosition;
			}	
			
		}
		public void ForceSetTargetPosition(Vector2 targetPosition)
		{
			if(player == null)
			{
				return;
			}
			NavigationAgent2D.TargetPosition = targetPosition;
			intervalTimer.Start(Directions.random.RandiRange(1 , 4));

		}
		public void FollowPath()
		{

			
			if(NavigationAgent2D.IsNavigationFinished())
			{
				EmitSignal(SignalName.NavigationFinished);
				velocityComponent.Decelerate();
				
			 	return;
			}
			var direction  = (NavigationAgent2D.GetNextPathPosition() - GlobalPosition ).Normalized();
			velocityComponent.AccelerateInDirection(direction);
			NavigationAgent2D.SetVelocity(velocityComponent.Velocity);
			
		}
		private void OnVelocityComputed(Vector2 velocity)
		{
			var newDirection = 	velocity.Normalized();
			var currentDirection = velocityComponent.Velocity.Normalized();
			var halfvay = newDirection.Lerp(currentDirection  , 1f - Mathf.Exp(velocityComponent.AccelerationCoeficient * (float)GetProcessDeltaTime())).Normalized();
			velocityComponent.Velocity = halfvay * velocityComponent.Velocity.Length();
		}
		
	}
}
