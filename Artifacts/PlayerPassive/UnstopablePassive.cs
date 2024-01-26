using Game.Components;
using Godot;
using System;

namespace PlayerPassive
{
	public partial class UnstopablePassive : Node2D , IVisitor
	{
		game_events _gameEvents;
		private HurtBoxComponent _hurtBoxComponent;
		private VelocityComponent _velocityComponent;
		Timer _resetTimer;
		private int _effectCount;
		private float _speedMultiplier = 0.1f;
		private int _arrmorMultiplier = 1;
		private const string SPEED_MODIFIRE = "unstoppable";
		private const int MAX_EFFECT_STACK = 4;
        public override void _Ready()
        {
			_resetTimer = GetNode<Timer>("ResetTimer");			
			_gameEvents = GetNode<game_events>("/root/GameEvents");
			_gameEvents.Connect(game_events.SignalName.OnEnemyDied  ,Callable.From((Vector2 pos , int enemy_cost_inBullets) => 
			{
				ApplyBaff();
			}));
			_resetTimer.Connect(Timer.SignalName.Timeout , Callable.From(()=>
			{	
				ResetBaffs();
			}));
			
        }
		private void ApplyBaff()
		{
			_resetTimer.Start();
			if(_effectCount < MAX_EFFECT_STACK)
			{
				_effectCount += 1;
				_velocityComponent.SetSpeedPercentModifire(SPEED_MODIFIRE , _speedMultiplier * _effectCount);
				_hurtBoxComponent.SetArrmor(_arrmorMultiplier *  _effectCount);
			}
			
			
		}
		private void ResetBaffs()
		{
			_resetTimer.Stop();
			_hurtBoxComponent.SetArrmor(_hurtBoxComponent.Armmor - (_arrmorMultiplier * _effectCount));
			_velocityComponent.SetSpeedPercentModifire(SPEED_MODIFIRE , 0 );
			_effectCount = 0;
		}

        public void Visit(HealthComponent _healthComponent){}
        public void Visit(WeaponRootComponent _weaponRootComponent){}

        public void Visit(HurtBoxComponent _hurtBoxComponent)
        {
           this._hurtBoxComponent = _hurtBoxComponent;
        }

        public void Visit(VelocityComponent _velocityComponent)
        {
			this._velocityComponent = _velocityComponent;
			_velocityComponent.AddSpeedPercentModifire(SPEED_MODIFIRE , 0);
        }

        public void Visit(StatusRecivierComponent _statusRecivierComponent)
        {
            
        }
    }

}
