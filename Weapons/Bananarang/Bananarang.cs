using Godot;
using Godot.NativeInterop;
using System;



namespace Game.Weapons
{
	public partial class Bananarang : Node2D , IWeapon 
	{
		private bool _canShoot = true;
		PackedScene _bananarangProjectile;
		ResourcePreloader _resourcePreloader;
		Sprite2D _bananarangImage;
        public override void _Ready()
        {
			_bananarangImage = GetNode<Sprite2D>("Sprite2D");
			_resourcePreloader = GetNode<ResourcePreloader>("ResourcePreloader");
			_bananarangProjectile = _resourcePreloader.GetResource("BananarangProjectile") as PackedScene;
        }
        public void Shoot(Vector2 directionToTarget)
        {
			Throw(directionToTarget);
		}
		private void Throw(Vector2 _direction)
		{
			_bananarangImage.Visible = false;
			_canShoot = false;
			CreateProjectile(_direction);
		}

		public void CreateProjectile(Vector2 _directionToTarget)
        {
			
            
			var _bananarangProjectileInstance = _bananarangProjectile.Instantiate() as BananarangProjectile;
			_bananarangProjectileInstance.Connect(BananarangProjectile.SignalName.TurnedBack , Callable.From(()=> OnTurnBack()));
			_bananarangProjectileInstance.SetProjectileDirection(_directionToTarget);
			_bananarangProjectileInstance.GlobalPosition = GlobalPosition;
			GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(_bananarangProjectileInstance);
        }
		private void OnTurnBack()
		{
			_bananarangImage.Visible = true;
			if(GetTree().GetNodesInGroup("bananarangsProjectils").Count <= 2)
			{
				_canShoot = true;
			}
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

