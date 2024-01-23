using Godot;
using System;
using Generation.Alghoritms;
using Game.Components;
using System.ComponentModel.DataAnnotations;


namespace Game.Weapons
{
	public partial class ShotGun : Weapon  , IWeapon
	{
		[Export] Timer timeToRecoilDecresment;
		[Export] AnimationPlayer animationPlayer;
		[Export] Timer atackDelayTimer;
		[Export] float MaxRecoil = 30;
		[Export] Marker2D shootPosition;
		[Export] private bool isEnemy = false;
		[Export] AudioStreamPlayer2D gunAudioPlayer;
		[Export] PackedScene bulletPartickle;
		
		float currentRecoil = 0;
		private WeaponStats shotGunStats;
		public override void _Ready()
		{
			base._Ready();
			GD.Print(this._weaponRootComponent);
			if(isEnemy)
			{
				shotGunStats = ResourceLoader.Load<WeaponStats>("res://Resourses/WeaponResourses/ShotGun/EnemyShootGun.tres");
			}
			else
			{
				shotGunStats = ResourceLoader.Load<WeaponStats>("res://Resourses/WeaponResourses/ShotGun/ShotGun.tres");
			}
		}
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
		public override void Shoot(Vector2 directionToTarget)
		{
			animationPlayer.Play("Shoot");
			gunAudioPlayer.Play();
			EmitBulletShelsParticle(bulletPartickle);
			_canShoot = false;
			var recoilIncreasment = MaxRecoil * 0.1f;
			var recoil_degree_max = currentRecoil * 0.5f;
			var recoil_rad_actual = Mathf.DegToRad(Directions.random.RandfRange(-recoil_degree_max , recoil_degree_max));
			currentRecoil = Mathf.Clamp(currentRecoil + recoilIncreasment ,0, MaxRecoil); 
			BaseBullet bulletInstance =  shotGunStats.Bullet.Instantiate() as BaseBullet;
			bulletInstance.ApplyAfexForBullet(Affex);
			bulletInstance.Position = shootPosition.GlobalPosition;
			bulletInstance.direction = directionToTarget.Rotated(recoil_rad_actual);
			bulletInstance.LookAt(directionToTarget);
			GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(bulletInstance);
			timeToRecoilDecresment.Start(2);
			atackDelayTimer.Start(shotGunStats.atack_deley);
		}
        public override void EmitBulletShelsParticle(PackedScene bulletParickle)
        {
            base.EmitBulletShelsParticle(bulletParickle);
        }

    }
}

