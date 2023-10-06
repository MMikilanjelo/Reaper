using GameUI;
using Godot;
using System;

namespace Game.Components
{
	public partial class HurtBoxComponent : Area2D
	{
		public const string GROUP_ENEMY_HURTBOX = "enemy_hitbox";
		private float dmg_Reduction_Multiplier = 1f;
		PackedScene floatingTextScene;
		[Export] private HealthComponent healthComponent;
		[Signal] public delegate void HitByHitBoxEventHandler(HitBoxComponent hitBoxComponent);

		public float DmgReductonMultiplier
		{
			get => dmg_Reduction_Multiplier;
			
			set{
				dmg_Reduction_Multiplier = Mathf.Clamp(value, 0 , 1);
				dmg_Reduction_Multiplier = value;
			}
		}
		public void SetDmgReductonMultiplier(float percent)
		{
			DmgReductonMultiplier -= percent;
		}
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
				var totaldmg = hitBoxComponent.dmg * dmg_Reduction_Multiplier;
				DealDmg(totaldmg);
				hitBoxComponent.OnHit();
				EmitSignal(SignalName.HitByHitBox , hitBoxComponent);
				var floating_text = floatingTextScene.Instantiate() as FloatingText;
				GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(floating_text);
				floating_text.GlobalPosition = GlobalPosition;
				floating_text.Start(Convert.ToString(totaldmg));
				
			}
		
			
		}
		private void DealDmg(float dmg)
		{
			
			healthComponent?.Damage(dmg);
		}
		
	}
}

