using Godot;
using System;
using Enemy.Parts;
using DotEffects;
using System.Collections.Generic;
using GameLogick.Utilities;

namespace Game.Components
{
	public partial class HitBoxComponent : Area2D
	{	
		[Export] public float dmg = 1;
		[Signal] public delegate void OnImpacktEventHandler();
		[Signal] public delegate void OnWallCollideEventHandler();
		[Export] public CollisionShape2D hitBoxArea;
		game_events Game_Events;
		CombatEvents combatEvents;
	

        public override void _Ready()
        {
			Game_Events = GetNode<game_events>("/root/GameEvents");
			combatEvents = GetNode<CombatEvents>("/root/CombatEvents");
			Connect(SignalName.BodyEntered , new Callable(this, nameof(onBodyEntered)));
			
        }
        
		public void  OnHit(HitInfo hitInfo)
		{
			combatEvents.OnHitByHitBox(hitInfo);
			EmitSignal(SignalName.OnImpackt);
		}

		private void onBodyEntered(Node2D Body)
		{
			EmitSignal(SignalName.OnWallCollide);
		}
		public void SetDisable(bool diseable)
		{
			hitBoxArea.Disabled = diseable;
		}
		public void EnableHitBox()
		{
			hitBoxArea.Disabled = false;
		}
		public void DisableHitBox()
		{
			hitBoxArea.Disabled = true;
		}
		public void EmitImpackt()
		{
			EmitSignal(SignalName.OnImpackt);
		}
	}
}

