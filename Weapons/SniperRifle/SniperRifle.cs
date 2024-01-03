using Godot;
using System;


namespace Game.Weapons
{
	public partial class SniperRifle : Weapon , IWeapon
	{
		[Export] private RayCast2D shootRay;
		[Export] Sprite2D crosshair;
        public override void _Ready()
        {
			
        }
        public override void _PhysicsProcess(double delta)
        {
			var _castPoint = shootRay.TargetPosition;
			shootRay.ForceRaycastUpdate();
			if(shootRay.IsColliding())
			{
				_castPoint = ToLocal(shootRay.GetCollisionPoint());
			}
			crosshair.Position = _castPoint;
		}
		public override void Shoot(Vector2 directionToTarget)	
        {
		}
    }
}

