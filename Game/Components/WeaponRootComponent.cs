using Godot;
using System;
using Game.Weapons;

namespace Game.Components
{
	public partial class WeaponRootComponent : Node2D
	{
		[Export] CharacterBody2D entity;
		[Export] Weapon CurrentWeapon;
		public PackedScene AffexToApply = null;
		[Signal] public delegate void ShotedFromWeaponEventHandler();
		public WeaponStats currentWeaponStats;
		private bool _HasAmmoRemaining = true;
		game_events game_Events;
        public override void _Ready()
        {
			game_Events = GetNode<game_events>("/root/GameEvents");	
	   		game_Events.Connect(game_events.SignalName.OnRunOutAmmo , Callable.From((bool _hasAmmmo)=> _HasAmmoRemaining = _hasAmmmo));
        }
		public void ChangeWeapon(PackedScene newWeapon)
		{
			
			CurrentWeapon.QueueFree();
			var Weapon_To_Change = newWeapon.Instantiate() as Weapon;
			AddChild(Weapon_To_Change);
			CurrentWeapon = Weapon_To_Change;
		}
		public void ShootFromCurrentWeapon(Vector2 directionToShoot)
		{
			if(!_HasAmmoRemaining || !CurrentWeapon._canShoot)
			{
				return;
			}
			EmitSignal(SignalName.ShotedFromWeapon);
			CurrentWeapon?.Shoot(directionToShoot);
		}
		public void AddAfexToWeapon(PackedScene AffexToAdd)
		{
			CurrentWeapon.Affex = AffexToAdd;
			CurrentWeapon.hitBoxComponent?.AddEffecToHit(CurrentWeapon.Affex);
		}

		
        
    }
}

