using Godot;
using System;
using Game.Components;
namespace Game.Weapons
{
	public partial class BaseBullet : CharacterBody2D 
	{
		[Export] protected  HitBoxComponent hitBoxComponent;
		public PackedScene afex {get ; set;}
		protected float MoveSpeed{get ; set;}
		protected Vector2  direction {get; set;}
		public virtual void ApplyAfexForBullet(PackedScene afex)
		{
			if(afex != null)
			{
				hitBoxComponent.AddEffecToHit(afex);
			}
		}
		public void SetBulletDirection(Vector2 _direction)
		{
			direction = _direction;
		}
	}
}

