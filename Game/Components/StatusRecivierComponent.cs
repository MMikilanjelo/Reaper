using Godot;
using System;
using GameLogick.Utilities;
using System.Reflection.Metadata.Ecma335;
using DotEffects;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Game.Components
{
	
	
	public partial class StatusRecivierComponent : Node2D  
	{
		CharacterBody2D entity;
		[Export] HurtBoxComponent hurtBoxComponent;
		[Signal] public delegate void OnForsedRemoveEfectEventHandler();
		private bool CanReciveEffect = true;

        public override void _Ready()
        {
			hurtBoxComponent.Connect(HurtBoxComponent.SignalName.HitByHitBox , Callable.From((HitBoxComponent hitBox)=>
			{
				ApplyEffect(hitBox.AtackAfex);
			}));
			entity = GetParent<CharacterBody2D>();
		}
		public void ApplyEffect(PackedScene effectToApply)
		{
			if(effectToApply == null || CanReciveEffect == false)
			{
				return;
			}
			var  _efect_recivier_data = new StatusEfffectData
			{
				healthComponent = entity.GetNode<HealthComponent>("HealthComponent"),
				statusRecivierComponent = this
			};
			var currentEffect = effectToApply.Instantiate() as BaseEffect;
			entity.AddChild(currentEffect);
			currentEffect.Connect(BaseEffect.SignalName.OnRemoveEfect  , Callable.From(()=>CanReciveEffect = true));
			currentEffect.ApplyEffect(_efect_recivier_data);
			CanReciveEffect = false;
		}
		public void ForcedRemoveEffect()
		{
			EmitSignal(SignalName.OnForsedRemoveEfect);
		}
	
        


    }
	public partial class StatusEfffectData : RefCounted
	{
		public HealthComponent healthComponent;
		public StatusRecivierComponent statusRecivierComponent;
	}
	
}
