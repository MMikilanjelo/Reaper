using Game.Components;
using Godot;
using System;

namespace PlayerPassive
{
	public partial class ShroudPassive : Node
	{
		public HurtBoxComponent hurtBoxComponent;
		private int _receivedAttackCount = 0;

        public override void _Ready()
        {
			hurtBoxComponent.HitByHitBox += (HitBoxComponent hitBox) => ApplyShroud();
			hurtBoxComponent.MissedAttack += () => RemoveShroud();
		}
		private void ApplyShroud()
		{
			_receivedAttackCount += 1;
			if(_receivedAttackCount >= 2)
			{
				_receivedAttackCount = 0;
				hurtBoxComponent.SetHitChance(-1);
			}
		}
		private void RemoveShroud()
		{
			hurtBoxComponent.SetHitChance(100);
		}
	
		

    }
}

