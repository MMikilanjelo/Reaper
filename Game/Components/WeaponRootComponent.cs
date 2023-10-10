using Godot;
using System;
using Game.Weapons;

namespace Game.Components
{
	public partial class WeaponRootComponent : Node2D
	{
		[Export] CharacterBody2D entity;
		[Export] Weapon CurrentWeapon;
		[Signal] public delegate void ShotedFromWeaponEventHandler();
		public WeaponStats currentWeaponStats;
		private bool _HasAmmoRemaining = true;
		game_events game_Events;
        public override void _Ready()
        {
			game_Events = GetNode<game_events>("/root/GameEvents");	
	   		game_Events.Connect(game_events.SignalName.OnRunOutAmmo , Callable.From((bool _hasAmmmo)=> _HasAmmoRemaining = _hasAmmmo));
        }
		public void ChangeWeapon(Weapon newWeapon)
		{
			CurrentWeapon.QueueFree();
			CurrentWeapon = newWeapon;
		}
		public void ShootFromCurrentWeapon(Vector2 directionToShoot)
		{
			if( _HasAmmoRemaining && CurrentWeapon._canShoot)
			{
				EmitSignal(SignalName.ShotedFromWeapon);
				CurrentWeapon.Shoot(directionToShoot);
			}
		}
		
        
    }
}

