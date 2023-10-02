using Godot;
using System;
namespace Game.Components
{
public partial class WeaponControllerComponent : Node2D
	{
		[Export] CharacterBody2D parent;
		IWeapon weapon;
		public override void _PhysicsProcess(double delta)
		{
			weapon.Shoot();
		}

	}
	public interface IWeapon
	{
		Resource weaponResourse{get;}
		void Shoot();
	}
}


