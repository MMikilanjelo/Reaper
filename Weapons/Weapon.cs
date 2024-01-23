using Godot;
using Game.Components;
using System;

namespace Game.Weapons
{
	public partial class Weapon : Node2D , IWeapon 
	{
		protected WeaponRootComponent _weaponRootComponent;
		public bool _canShoot = true;
		public PackedScene Affex = null;
		public HitBoxComponent hitBoxComponent = null;
        public override void _Ready()
        {
            //_weaponRootComponent = GetTree().GetFirstNodeInGroup("WeaponRootComponent") as WeaponRootComponent;
        }
        public virtual void Shoot(Vector2 directionToTarget)
		{
			
		}
		public virtual void EmitBulletShelsParticle(PackedScene bulletParickle)
		{	
			GpuParticles2D 	patickles = bulletParickle.Instantiate() as GpuParticles2D;
			patickles.Position = Position;
			this.AddChild(patickles);
		}
		public void OnWeaponChanged()
		{
			QueueFree();
		}


    }
	public interface IWeapon
	{
		void Shoot(Vector2 directionToTarget);
		
	}
	public partial class BaseBullet : CharacterBody2D 
	{
		[Export] protected  HitBoxComponent hitBoxComponent;
		public PackedScene afex {get ; set;}
		public float MoveSpeed{get ; set;}
		public Vector2  direction {get; set;}
		public virtual void ApplyAfexForBullet(PackedScene afex)
		{
			if(afex != null)
			{
				hitBoxComponent.AddEffecToHit(afex);
			}
		}
	}
}

