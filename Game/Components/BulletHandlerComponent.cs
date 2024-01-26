using Godot;
using System;
using Generation.Alghoritms;


namespace Game.Components
{
	public partial class BulletHandlerComponent : Node2D
	{
		[Export] PackedScene Bullet;
		[Export] Timer timeToRecoilDecresment;
		[Export] float MaxRecoil = 30;
		float currentRecoil = 0;
		
        
        public override void _PhysicsProcess(double delta)
        {
			if(timeToRecoilDecresment.IsStopped())
			{
				var recoil_increment = MaxRecoil * 0.1f;
				currentRecoil = Mathf.Clamp(currentRecoil - recoil_increment , 0 ,	MaxRecoil);

			}
        }
        public void Shoot( CharacterBody2D Parent, Vector2 directionToTarget , Vector2 PostionToInstantiate , float MoveSpeed)
		{
			// var recoilIncreasment = MaxRecoil * 0.1f;
			// var recoil_degree_max = currentRecoil * 0.5f;
			// var recoil_rad_actual = Mathf.DegToRad(Directions.random.RandfRange(-recoil_degree_max , recoil_degree_max));
			// currentRecoil = Mathf.Clamp(currentRecoil + recoilIncreasment ,0, MaxRecoil); 
			// Bullet bulletInstance =  Bullet.Instantiate() as Bullet;
			// bulletInstance.Position = PostionToInstantiate;
			// bulletInstance.direction = directionToTarget.Rotated(recoil_rad_actual);
			// bulletInstance.MoveSpeed = MoveSpeed;
			// bulletInstance.LookAt(GetGlobalMousePosition());
			// GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(bulletInstance);
			// timeToRecoilDecresment.Start(2);
		}
	}

}
