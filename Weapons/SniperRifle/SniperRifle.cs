using Game.Components;
using Godot;
using System;


namespace Game.Weapons
{
	public partial class SniperRifle : Weapon , IWeapon
	{
		[Export] private RayCast2D shootRay;
		[Export] Sprite2D crosshair;
		[Export] Line2D aimLine;
		SegmentShape2D hitBoxShape;
		CollisionShape2D hitBoxArea;
        public override void _Ready()
        {
			hitBoxArea = GetNode<CollisionShape2D>("Sprite2D/Marker2D/HitBoxComponent/CollisionShape2D");
			hitBoxShape = hitBoxArea.Shape as SegmentShape2D;

			_canShoot = true;
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
			hitBoxShape.B = _castPoint;
			aimLine.SetPointPosition(1,_castPoint);
		}
			
		public override void Shoot(Vector2 directionToTarget)	
        {
			Appear();
		}
		private void Appear()
		{
			Tween tween = CreateTween();
			hitBoxArea.Disabled = false;
			_canShoot = false;
			tween.TweenProperty(aimLine , "width" , 2f , 0.1f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Circ);
			tween.TweenCallback(Callable.From(()=> 
			{
				hitBoxArea.Disabled = true;
			}));
			tween.Chain();
			tween.TweenProperty(aimLine , "width" , 0 , 1.4f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Circ);
			tween.TweenCallback(Callable.From(()=> 
			{

				_canShoot = true;
			}));
		}
		private void DisAppear()
		{
			
		}

	}
}

