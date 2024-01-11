using GameUI;
using Godot;
using GameLogick.Utilities;
using System.Security.Cryptography.X509Certificates;

namespace Game.Components
{
	public partial class HurtBoxComponent : Area2D
	{
		RandomNumberGenerator random;
		game_events Game_Events;
		public const string GROUP_ENEMY_HURTBOX = "enemy_hitbox";
		private float dmg_Reduction_Multiplier = 0f;
		[Export]private int armmor = 0;
		private float miss_chance = 0;
		private int _hitchance = 100;
		PackedScene floatingTextScene;
		[Export] private RandomAudioPlayer audioPlayer;
		[Export] private HealthComponent healthComponent;
		[Signal] public delegate void HitByHitBoxEventHandler(HitBoxComponent hitBoxComponent);
		[Signal] public delegate void MissedAttackEventHandler();
		[Signal] public delegate void MissChanceChangedEventHandler(int miss_chance);

		
		public float MissChance
		{
			get => miss_chance;
			private set{
				miss_chance = Mathf.Clamp(value , 0 , 100);
				EmitSignal(SignalName.MissChanceChanged , miss_chance);
			}
		}
		public int HitChance
		{
			get => _hitchance;
			private set{
				_hitchance = Mathf.Clamp(value , 0 , 100);
			}
		}
		public float DmgReductonMultiplier
		{
			get => dmg_Reduction_Multiplier;
			private set{
				dmg_Reduction_Multiplier = Mathf.Clamp(value, 0 , 1);
				
			}
		}
		public int Armmor{
			get => armmor;
			private set {
				armmor = value;
			}
		}
		public void SetDmgReductonMultiplier(float percent)
		{
			DmgReductonMultiplier += percent;
		}
		public void SetArrmor(int value)
		{
			Armmor = value;
		}
		public void SetMissChance(float amount)
		{
			MissChance += amount;
		}
		public void SetHitChance(int amount)
		{
			_hitchance = amount;
		}
		public override void _Ready()
		{
			Game_Events = GetNode<game_events>("/root/GameEvents");
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
				
				var hitChance = random.RandiRange(0 , _hitchance);
				if(hitChance > miss_chance)
				{
					audioPlayer.PlayRandom();
					var totaldmg = CalculateIncomingDamage(hitBoxComponent.dmg , dmg_Reduction_Multiplier , armmor);
					DealDmg(totaldmg);
					hitBoxComponent.OnHit();
					EmitSignal(SignalName.HitByHitBox , hitBoxComponent);
					if(Owner is  PlayerController)
					{
						return;
					}
					Game_Events.EmitDmgRecivedByEnemy(totaldmg);
					
				}
				else 
				{
					EmitSignal(SignalName.MissedAttack);
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

