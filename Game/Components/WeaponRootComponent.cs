using Godot;
using System;
using Game.Weapons;

namespace Game.Components
{
	public partial class WeaponRootComponent : Node2D , IVisitable
	{
		public IWeapon CurrentWeapon;
		[Export] Node2D WeaponNode;
		[Export] CharacterBody2D entity;
		[Signal] public delegate void ShotedFromWeaponEventHandler(Vector2 _direction );
		[Signal] public delegate void WeaponChangedEventHandler();
		game_events game_Events;
		private bool _HasAmmoRemaining = true;

        public override void _Ready()
        {
			CurrentWeapon = WeaponNode as IWeapon;
			game_Events = GetNode<game_events>("/root/GameEvents");	
	   		game_Events.Connect(game_events.SignalName.OnRunOutAmmo , Callable.From((bool _hasAmmmo)=> _HasAmmoRemaining = _hasAmmmo));
        }

		public void ChangeWeapon(PackedScene _weaponScene)
		{
			if(CurrentWeapon != null)
			{
				CurrentWeapon.OnWeaponChanged();
				CurrentWeapon = null;
			}
			var Weapon_To_Change = _weaponScene.Instantiate() as Node2D;
			AddChild(Weapon_To_Change);
			CurrentWeapon = Weapon_To_Change as IWeapon;
		}

		public void ShootFromCurrentWeapon(Vector2 directionToShot)
		{
			if(!_HasAmmoRemaining) return;
			if(CurrentWeapon.IsReadyToShot())
			{
				EmitSignal(SignalName.ShotedFromWeapon , directionToShot);
				CurrentWeapon?.Shoot(directionToShot);
			}
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

