using GameUI;
using Godot;
using GameLogick.Utilities;

namespace Game.Components
{
	public partial class HurtBoxComponent : Area2D
	{
		RandomNumberGenerator random;
		public const string GROUP_ENEMY_HURTBOX = "enemy_hitbox";
		private float dmg_Reduction_Multiplier = 0f;
		private int armmor = 0;
		private float miss_chance = 0;
		PackedScene floatingTextScene;
		[Export] private HealthComponent healthComponent;
		[Signal] public delegate void HitByHitBoxEventHandler(HitBoxComponent hitBoxComponent);
		public float MissChance
		{
			get => miss_chance;
			private set{
				miss_chance = Mathf.Clamp(value , 0 , 100);
			}
		}
		public float DmgReductonMultiplier
		{
			get => dmg_Reduction_Multiplier;
			private set{
				dmg_Reduction_Multiplier = Mathf.Clamp(value, 0 , 1);
			}
		}
		public void SetDmgReductonMultiplier(float percent)
		{
			DmgReductonMultiplier += percent;
		}
		public void SetMissChance(float amount)
		{
			MissChance += amount;
		}
		public override void _Ready()
		{
			random = MathUtil.RNG;
			floatingTextScene = ResourceLoader.Load("res://UI/FloatingText.tscn") as PackedScene;
			if(CollisionLayer == 2)
			{
				AddToGroup(GROUP_ENEMY_HURTBOX);
				
			}
			Connect("area_entered" , new Callable(this, nameof(onAreaEntered)));
			
		}
		private bool canAceptBulletColisions()
		{
			return  healthComponent?._HasHealthRamaining ?? true;
		}
		private void onAreaEntered(Area2D oterArea)
		{
			if(oterArea is HitBoxComponent hitBoxComponent)
			{
				
				var hitChance = random.RandiRange(0 , 100);
				if(hitChance >= miss_chance)
				{
					
					var totaldmg = CalculateIncomingDamage(hitBoxComponent.dmg , dmg_Reduction_Multiplier , armmor);
					DealDmg(totaldmg);
					hitBoxComponent.OnHit();
					EmitSignal(SignalName.HitByHitBox , hitBoxComponent);
						
				}
				else 
				{
					hitBoxComponent.EmitImpackt();
					var Missed_text = floatingTextScene.Instantiate() as FloatingText;
					GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(Missed_text);
					Missed_text.GlobalPosition = GlobalPosition;
					Missed_text.Start("Missed");
				}
				
			
				
			}
		}
		private void DealDmg(float dmg)
		{
			healthComponent?.Damage(dmg);
		}
		private int CalculateIncomingDamage(float dmg  , float dmg_Reduction_Multiplier , int armmor = 0)
		{
			
			return Mathf.CeilToInt(dmg - (dmg * dmg_Reduction_Multiplier) - armmor); 
		}
	}
	
}

