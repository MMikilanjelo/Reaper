using Godot;
using System;


namespace Game.Components
{
	public partial class HitFlashComponent : Node
	{
		[Export] HealthComponent healthComponent;
		[Export] Sprite2D sprite2D;
		[Export] ShaderMaterial hit_flash_material;
		Tween hit_flash_tween;

        public override void _Ready()
        {
			sprite2D.Material = hit_flash_material;
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
			(sprite2D.Material as ShaderMaterial).SetShaderParameter("lerp_percent" , 1.0);
			hit_flash_tween = CreateTween();
			hit_flash_tween.TweenProperty(sprite2D.Material,"shader_parameter/lerp_percent" , 0.0 , .2);
		}
    }

}
