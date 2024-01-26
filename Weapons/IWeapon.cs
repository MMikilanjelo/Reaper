using Godot;
using Game.Components;
using System;

namespace Game.Weapons
{
	public interface IWeapon
	{
		void Shoot(Vector2 _directionToTarget);
		void OnWeaponChanged();
		bool IsReadyToShot();
	}

}

