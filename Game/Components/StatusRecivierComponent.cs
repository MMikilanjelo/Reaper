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
		[Export] HurtBoxComponent entityHurtBoxComponent;
		[Export] HealthComponent healthComponent;
		[Signal] public delegate void EffectAppliedEventHandler(StatusEffectData _data);
        public override void _Ready()
        {
			entity = GetParent<CharacterBody2D>();
			entityHurtBoxComponent.Connect(HurtBoxComponent.SignalName.HitByHitBox , Callable.From((HitBoxComponent hitBox)=>
				ApplyEffect(hitBox.effect)
			));

		}
		public void ApplyEffect(PackedScene effect)
		{
			var currentEffect = effect.Instantiate() as Node2D;
			entity.AddChild(currentEffect);
		}
        


    }
	
}
