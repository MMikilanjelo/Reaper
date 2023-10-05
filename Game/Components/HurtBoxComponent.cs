using GameUI;
using Godot;
using System;

namespace Game.Components
{
	public partial class HurtBoxComponent : Area2D
	{
		public const string GROUP_ENEMY_HURTBOX = "enemy_hitbox";
		PackedScene floatingTextScene;
		[Export] private HealthComponent healthComponent;
		[Signal] public delegate void HitByHitBoxEventHandler(HitBoxComponent hitBoxComponent);

		public override void _Ready()
		{
			floatingTextScene = ResourceLoader.Load("res://UI/FloatingText.tscn") as PackedScene;

			if(CollisionLayer == 1)
			{
				AddToGroup(GROUP_ENEMY_HURTBOX);
				Connect("area_entered" , new Callable(this, nameof(onAreaEntered)));
			}
			
		}
		private bool canAceptBulletCOlisions()
		{
			return  healthComponent?._HasHealthRamaining ?? true;
		}
		private void onAreaEntered(Area2D oterArea)
		{
			if(oterArea is HitBoxComponent hitBoxComponent)
			{
				
				DealDmg(hitBoxComponent.dmg);
				hitBoxComponent.OnHit();
				EmitSignal(SignalName.HitByHitBox , hitBoxComponent);
				var floating_text = floatingTextScene.Instantiate() as FloatingText;
				GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(floating_text);
				floating_text.GlobalPosition = GlobalPosition;
				floating_text.Start(Convert.ToString(hitBoxComponent.dmg));
				
			}
		
			
		}
		private void DealDmg(float dmg)
		{
			
			healthComponent?.Damage(dmg);
		}
		
	}
}

