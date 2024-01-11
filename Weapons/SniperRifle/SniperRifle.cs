using Game.Components;
using Godot;
using System;


namespace Game.Weapons
{
	public partial class SniperRifle : Weapon , IWeapon
	{
		[Export] private RayCast2D shootRay;
		[Export] Line2D aimLine;
		[Export] private float _atackDelay = 1.5f;
		
		AudioStreamPlayer2D soundPlayer;
		SegmentShape2D hitBoxShape;
		CollisionShape2D hitBoxArea;
		public override void _Ready()
		{
			hitBoxArea = GetNode<CollisionShape2D>("SniperRifle/Marker2D/HitBoxComponent/CollisionShape2D");
			hitBoxShape = hitBoxArea.Shape as SegmentShape2D;
			soundPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
			hitBoxComponent = GetNode<HitBoxComponent>("SniperRifle/Marker2D/HitBoxComponent");
		}
		public override void _PhysicsProcess(double delta)
		{
			var _castPoint = shootRay.TargetPosition;
			shootRay.ForceRaycastUpdate();
			if(shootRay.IsColliding())
			{
				_castPoint = ToLocal(shootRay.GetCollisionPoint());
			}
			hitBoxShape.B = _castPoint;
			aimLine.SetPointPosition(1,_castPoint);
		}
				
		public override void Shoot(Vector2 directionToTarget)	
		{
			GetTree().CreateTimer(_atackDelay).Connect(Timer.SignalName.Timeout , Callable.From(()=>
			{
				_canShoot = true;
			}));
			_canShoot = false;
			Appear();
				
		}
		private void Appear()
		{
			soundPlayer.Play();
			Tween tween = CreateTween();
			hitBoxArea.Disabled = false;
			tween.TweenProperty(aimLine , "width" , 2f , 0.1f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Circ);
			tween.TweenCallback(Callable.From(()=> {hitBoxArea.Disabled = true;}));
			tween.Chain();
			tween.TweenProperty(aimLine , "width" , 0 , 0.2f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Circ);
		}
		

	}
}

