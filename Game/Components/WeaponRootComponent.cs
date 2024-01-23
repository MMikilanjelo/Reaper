using Godot;
using System;
using Game.Weapons;

namespace Game.Components
{
	public partial class WeaponRootComponent : Node2D
	{
		[Export] CharacterBody2D entity;
		[Export] Weapon CurrentWeapon;
		[Signal] public delegate void ShotedFromWeaponEventHandler(Vector2 _direction);
		[Signal] public delegate void WeaponChangedEventHandler();
		private bool _HasAmmoRemaining = true;
		game_events game_Events;
        public override void _Ready()
        {
			
			game_Events = GetNode<game_events>("/root/GameEvents");	
	   		game_Events.Connect(game_events.SignalName.OnRunOutAmmo , Callable.From((bool _hasAmmmo)=> _HasAmmoRemaining = _hasAmmmo));
			game_Events.Connect(game_events.SignalName.WeaponShopSlotPurchased , new Callable(this , nameof(ChangeWeapon)));
        }
		private void ChangeWeapon(ShopSlotData shopSlotData)
		{
			if(CurrentWeapon != null)
			{
				CurrentWeapon.OnWeaponChanged();
				CurrentWeapon = null;
			}
			var Weapon_To_Change = shopSlotData._itemScene.Instantiate() as Weapon;
			AddChild(Weapon_To_Change);
			CurrentWeapon = Weapon_To_Change as Weapon;
		}

		public void ShootFromCurrentWeapon(Vector2 directionToShot)
		{
			if(!_HasAmmoRemaining || !CurrentWeapon._canShoot)
			{
				return;
			}
			EmitSignal(SignalName.ShotedFromWeapon , directionToShot);
			CurrentWeapon?.Shoot(directionToShot);
		}
		public void AddAfexToWeapon(PackedScene AffexToAdd)
		{
			CurrentWeapon.Affex = AffexToAdd;
			CurrentWeapon.hitBoxComponent?.AddEffecToHit(CurrentWeapon.Affex);
		}

		
        
    }
}

