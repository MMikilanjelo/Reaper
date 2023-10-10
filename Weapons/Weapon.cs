using Godot;
using System;

namespace Game.Weapons
{
	public partial class Weapon : Node2D , IWeapon
	{
		public bool _canShoot;
		public virtual void Shoot(Vector2 directionToTarget )
		{
			
		}
	}
	public interface IWeapon
	{
		void Shoot(Vector2 directionToTarget );
	}
}

