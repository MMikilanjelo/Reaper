using Godot;
using System;
using Generation.Alghoritms;
using Game.Components;
using System.ComponentModel.DataAnnotations;


namespace Game.Weapons
{
	public partial class ShotGun : Node2D, IWeapon
	{
		[Export] Timer timeToRecoilDecresment;
		[Export] AnimationPlayer animationPlayer;
		[Export] Timer atackDelayTimer;
		[Export] float MaxRecoil = 30;
		[Export] Marker2D shootPosition;
		[Export] AudioStreamPlayer2D gunAudioPlayer;
		[Export] PackedScene bulletPartickle;
		private bool _canShoot;
		float currentRecoil = 0;
		[Export] private WeaponStats shotGunStats;
		public override void _PhysicsProcess(double delta)
		{
				
			if(timeToRecoilDecresment.IsStopped())
			{
				var recoil_increment = MaxRecoil * 0.1f;
				currentRecoil = Mathf.Clamp(currentRecoil - recoil_increment , 0 ,	MaxRecoil);

			}
			if(atackDelayTimer.IsStopped()){
				_canShoot = true;
			}
		}
		public  void Shoot(Vector2 directionToTarget)
		{
			_canShoot = false;
			animationPlayer.Play("Shoot");
			gunAudioPlayer.Play();
			EmitBulletShelsParticle(bulletPartickle);
			var recoilIncreasment = MaxRecoil * 0.1f;
			var recoil_degree_max = currentRecoil * 0.5f;
			var recoil_rad_actual = Mathf.DegToRad(Directions.random.RandfRange(-recoil_degree_max , recoil_degree_max));
			currentRecoil = Mathf.Clamp(currentRecoil + recoilIncreasment ,0, MaxRecoil); 
			BaseBullet bulletInstance =  shotGunStats.Bullet.Instantiate() as BaseBullet;
			
			//bulletInstance.ApplyAfexForBullet(Affex);
			
			
			bulletInstance.Position = shootPosition.GlobalPosition;
			bulletInstance.SetBulletDirection(directionToTarget.Rotated(recoil_rad_actual));
			bulletInstance.LookAt(directionToTarget);
			GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(bulletInstance);
			timeToRecoilDecresment.Start(2);
			atackDelayTimer.Start(shotGunStats.atack_deley);
		}
        public  void EmitBulletShelsParticle(PackedScene bulletParickle)
        {
			GpuParticles2D 	patickles = bulletParickle.Instantiate() as GpuParticles2D;
			patickles.Position = Position;
			AddChild(patickles);
        }
		public void OnWeaponChanged()
		{
			QueueFree();
		}

        public bool IsReadyToShot()
        {
			return _canShoot;
        }


    }
}

