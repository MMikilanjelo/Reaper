using Godot;
using System;
using Game.Weapons;

namespace Game.Components
{
	public partial class WeaponRootComponent : Node2D , IVisitable
	{
		[Export] CharacterBody2D entity;
		[Export] Weapon CurrentWeapon;
		[Signal] public delegate void ShotedFromWeaponEventHandler(Vector2 _direction);
		[Signal] public delegate void WeaponChangedEventHandler();
		game_events game_Events;
		private bool _HasAmmoRemaining = true;

        public override void _Ready()
        {
			
			game_Events = GetNode<game_events>("/root/GameEvents");	
	   		game_Events.Connect(game_events.SignalName.OnRunOutAmmo , Callable.From((bool _hasAmmmo)=> _HasAmmoRemaining = _hasAmmmo));
        }
		public void ChangeWeapon(PackedScene _itemScene)
		{
			
			if(CurrentWeapon != null)
			{
				CurrentWeapon.OnWeaponChanged();
				CurrentWeapon = null;
			}
			var Weapon_To_Change = _itemScene.Instantiate() as Weapon;
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
			GD.Print("AddAfexToWeapon");
			CurrentWeapon.Affex = AffexToAdd;
			CurrentWeapon.hitBoxComponent?.AddEffecToHit(CurrentWeapon.Affex);
		}
		public void AdditionalShoot(Vector2 _directionToShot)
		{
			CurrentWeapon?.Shoot(_directionToShot);
		}
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

    }
}

