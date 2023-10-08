using Godot;
using System;
using GameLogick.Utilities;
using System.Reflection.Metadata.Ecma335;
using DotEffects;
using System.Collections.Generic;

namespace Game.Components
{
	
	
	public partial class StatusRecivierComponent : Node2D  
	{
		CharacterBody2D entity;
		game_events Game_Events;
		CombatEvents CombatEvents;
        public override void _Ready()
        {
			Game_Events = GetNode<game_events>("/root/GameEvents");
			CombatEvents = GetNode<CombatEvents>("/root/CombatEvents");
			CombatEvents.Connect(CombatEvents.SignalName.ApplyAffexToHittedEntity , Callable.From((PackedScene efectToApply , HitInfo hitInfo)=> 
			ApplyEffectUsingGameEvents(efectToApply , hitInfo)));
			entity = GetParent<CharacterBody2D>();
		}
		public void ApplyEffectUsingGameEvents(PackedScene effectToApply , HitInfo hitInfo)
		{
			var  _efect_recivier_data = new StatusEfffectData
			{
				healthComponent = hitInfo.hittedHealthComponent
			};
			var currentEffect = effectToApply.Instantiate() as BaseEffect;
			entity.AddChild(currentEffect);
			currentEffect.ApplyEffect(_efect_recivier_data);
		}
        


    }
	public partial class StatusEfffectData : RefCounted
	{
		public HealthComponent healthComponent;
	}
	
}
