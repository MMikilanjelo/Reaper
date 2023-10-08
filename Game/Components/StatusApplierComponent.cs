using Godot;
using System;


namespace Game.Components
{
	public partial class StatusApplierComponent : Node2D
	{
		
		[Export] PackedScene effectToApllyScene;
		[Signal] public delegate void ApplyEffectEventHandler(PackedScene efectToApply);
		game_events Game_Events;
		CombatEvents combatEvents;
        public override void _Ready()
        {
			Game_Events = GetNode<game_events>("/root/GameEvents");
			combatEvents = GetNode<CombatEvents>("/root/CombatEvents");
			combatEvents.Connect(CombatEvents.SignalName.HittedByHitBox , Callable.From((HitInfo hitInfo) => ApplyAffexOnHit(hitInfo)));
	    }
		private void ApplyAffexOnHit(HitInfo hitInfo)
		{
			combatEvents.Apply_Affex_To_HittedEntity(effectToApllyScene , hitInfo);
		}
		

    }
}

