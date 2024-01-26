using Game.Components;
using Godot;
using System;

namespace PlayerPassive
{
	public partial class ShroudPassive : Node2D , IVisitor
	{
		private HurtBoxComponent _hurtBoxComponent;
		private int _receivedAttackCount = 0;
		private void ApplyShroud()
		{
			_receivedAttackCount += 1;
			if(_receivedAttackCount >= 2)
			{
				_receivedAttackCount = 0;
				_hurtBoxComponent.SetHitChance(-1);
			}
		}
		private void RemoveShroud()
		{
			_hurtBoxComponent.SetHitChance(100);
		}
		public void Visit(HurtBoxComponent _hurtBoxComponent)
		{
			this._hurtBoxComponent = _hurtBoxComponent;
			_hurtBoxComponent.Connect(HurtBoxComponent.SignalName.HitByHitBox , Callable.From((HitBoxComponent _hitBox)=>
			{
				ApplyShroud();
			}));
			_hurtBoxComponent.Connect(HurtBoxComponent.SignalName.MissedAttack, Callable.From(()=>
			{
				RemoveShroud();
			}));
        }
		public void Visit(HealthComponent _healthComponent){}
		public void Visit(VelocityComponent _velocityComponent){}

        public void Visit(WeaponRootComponent _weaponRootComponent){}

        public void Visit(StatusRecivierComponent _statusRecivierComponent){}
    }
}

