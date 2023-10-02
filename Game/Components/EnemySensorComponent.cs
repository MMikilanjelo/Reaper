using Godot;
using System;
using System.Diagnostics;

namespace Game.Components
{
	public partial class EnemySensorComponent : Area2D
{
		public bool isInRange{get;private set;} = false;
		[Signal] public delegate void PlayerEnteredInRangeEventHandler();
		[Signal] public delegate void WallInRangeEventHandler();
		[Signal] public delegate void PlayerLeavedDetectionRangeEventHandler();
		public override void _Ready()
		{
			Connect(Area2D.SignalName.BodyEntered  , Callable.From((Node2D body)=>
			{
				if(body is PlayerController)
				{
					EmitSignal(SignalName.PlayerEnteredInRange);
					isInRange = true;
				}
			}));
			Connect(Area2D.SignalName.BodyExited  , Callable.From((Node2D body)=>
			{
				if(body is PlayerController)
				{
					EmitSignal(SignalName.PlayerLeavedDetectionRange);
					isInRange = false;
				}
			}));
			
		}
	
}
}

