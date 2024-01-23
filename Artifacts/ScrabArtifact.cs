using Godot;
using System;
using Game.Components;
public partial class ScrabArtifact : Node2D , IVisitor
{
	private game_events _gameEvemts;
	private ResourcePreloader _resourcePreloader;
    public override void _Ready()
    {
		_gameEvemts = GetNode<game_events>("/root/GameEvents");
    }
	#region  IVistor Implementation

	public void Visit(HealthComponent _healthComponent){}
	public void Visit(HurtBoxComponent _hurtBoxComponent){}
	public void Visit(VelocityComponent _velocityComponent){}
    
	
	public void Visit(WeaponRootComponent _weaponRootComponent)
    {
        _weaponRootComponent.Connect(WeaponRootComponent.SignalName.ShotedFromWeapon , Callable.From((Vector2 _direction) =>
		{
			var _degrees = Mathf.DegToRad(15);
			_weaponRootComponent.AdditionalShoot(_direction.Rotated(_degrees));
		}));
    }
    #endregion
}
