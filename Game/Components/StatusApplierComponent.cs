using Godot;
using System;


namespace Game.Components
{
	public partial class StatusApplierComponent : Node2D
	{
		
		[Export] PackedScene effectToApllyScene;
		[Signal] public delegate void ApplyEffectEventHandler(PackedScene efectToApply);
		game_events Game_Events;
        public override void _Ready()
        {
			Game_Events = GetNode<game_events>("/root/GameEvents");
			Game_Events.Connect(game_events.SignalName.OnHitByHitBox , Callable.From((HitInfo hitInfo)=>ApplyAffexOnHit(hitInfo)));
	    }
		private void ApplyAffexOnHit(HitInfo hitInfo)
		{
			Game_Events.OnApplyAfexToEntity(effectToApllyScene ,hitInfo );
		}
		

    }
}

