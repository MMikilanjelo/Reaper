using Godot;
using System;


namespace Game.Components
{
	public partial class HitFlashComponent : Node
	{
		[Export] HealthComponent healthComponent;
		[Export] Node2D visualsNode;
		[Export] ShaderMaterial hit_flash_material;
		Tween hit_flash_tween;

        public override void _Ready()
        {
			
			healthComponent.Connect(HealthComponent.SignalName.HealthChanged ,Callable.From((HealthComponent.HealthUpdate healthUpdate)=>
			{
				if(healthUpdate.CurrentHealth == healthUpdate.MaxHealth)
				{
					return;
				}
				AppyMaterial();
			}));
			
        }
		private void AppyMaterial()
		{
			if(hit_flash_tween != null  && hit_flash_tween.IsValid())
			{
				hit_flash_tween.Kill();
			}
			visualsNode.Material = hit_flash_material;	
			(visualsNode.Material as ShaderMaterial).SetShaderParameter("lerp_percent" , 1.0);
			hit_flash_tween = CreateTween();
			hit_flash_tween.TweenProperty(visualsNode.Material,"shader_parameter/lerp_percent" , 0.0 , .2);
		}
    }

}
