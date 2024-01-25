using System.Security.Cryptography;
using Game.Components;
using Godot;

namespace PlayerPassive
{
	
	public partial class LifeStealPassive : Node2D , IVisitor
	{
		private game_events _gameEvents;
		private HealthComponent _healthComponent;
		private int _lifeStealAmount = 30;
		public override void _Ready()
        {
			_gameEvents = GetNode<game_events>("/root/GameEvents");
			_gameEvents.Connect(game_events.SignalName.OnEnemyDied  ,Callable.From((Vector2 pos , int enemy_cost_inBullets) => 
			{
				ApplyLifeSteal(_healthComponent);
			}));
		}
		public void Visit(HealthComponent _healthComponent)
        {
			this._healthComponent = _healthComponent;
        }
		private void ApplyLifeSteal(HealthComponent _healthComponent)
		{
			_healthComponent.IncreaseCurrentHealth(_lifeStealAmount);
		}
        public void Visit(HurtBoxComponent _hurtBoxComponent) {}

        public void Visit(VelocityComponent _velocityComponent){}

        public void Visit(WeaponRootComponent _weaponRootComponent){}
        public override void _ExitTree()
        {
        }
    }
}
