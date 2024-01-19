using Godot;
using DotEffects;
using System.Collections.Generic;
namespace Game.Components
{
	
	
	public partial class StatusRecivierComponent : Node2D  
	{
		CharacterBody2D entity;
		[Export] HurtBoxComponent hurtBoxComponent;
		[Export] Node2D visualsNode;
		[Signal] public delegate void OnForsedRemoveEfectEventHandler();
		private bool _canReciveEffect = true;
		public bool CanReciveEffect
		{
			get => _canReciveEffect;
			private set{
				_canReciveEffect = value;
			}
		}
		private readonly Dictionary<PackedScene , BaseEffect> currentEffects = new Dictionary<PackedScene, BaseEffect>();
		// TODO: keep track of effects that uplied to enemy and update them if we try to add some sort of effect
		

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
			
			if(effectToApply == null || _canReciveEffect == false)
			{
				return;
			}
			if(!currentEffects.ContainsKey(effectToApply))
			{
				var currentEffect = effectToApply.Instantiate() as BaseEffect;
				var  _efect_recivier_data = new StatusEfffectData
				{
					healthComponent = entity.GetNode<HealthComponent>("HealthComponent"),
					statusRecivierComponent = this,
					entity = GetParent<CharacterBody2D>(),
					visuals = visualsNode
				};
				
				
				entity.AddChild(currentEffect);
				currentEffect.Connect(BaseEffect.SignalName.OnRemoveEfect  , Callable.From((BaseEffect effect)=>
				{
					foreach(var applied_effect in currentEffects)
					{
						if(applied_effect.Value == effect)
						{
							currentEffects.Remove(applied_effect.Key);
						}
					}
					//GD.Print(currentEffects.Count);
				}));
				currentEffect.ApplyEffect(_efect_recivier_data);
				currentEffects.Add(effectToApply , currentEffect);
				
			}
			else {
				if(currentEffects[effectToApply].effectStatsData.isStackable)
				{
					currentEffects[effectToApply].UpdateEffect();
				}
				else{
					//GD.Print("Cant update");
				}
				
			}

			
		
		}
		public void ForcedRemoveEffect()
		{
			EmitSignal(SignalName.OnForsedRemoveEfect);
		}
		public void SetReciveEffect(bool value)
		{
			CanReciveEffect = value;
		}
	
        


    }
	public partial class StatusEfffectData : RefCounted
	{
		public HealthComponent healthComponent;
		public StatusRecivierComponent statusRecivierComponent;
		public CharacterBody2D entity;
		public Node2D visuals;
	}
	
}
