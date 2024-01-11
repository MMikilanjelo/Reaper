using Game.Components;
using Godot;
using System;

namespace PlayerPassive
{
	public partial class UnstopablePassive : Node
	{
		game_events Game_Events;
		[Export]public HurtBoxComponent hurtBoxComponent;
		[Export]public VelocityComponent velocityComponent;
		[Export] Timer resetTimer;
		private int _effectCount;
		private float _speedMultiplier = 0.1f;
		private int _arrmorMultiplier = 1;
		private const string SPEED_MODIFIRE = "unstoppable";
		private const int MAX_EFFECT_STACK = 6;
        public override void _Ready()
        {
			//To do fix move speed multipliers  ) 
			Game_Events = GetNode<game_events>("/root/GameEvents");
			Game_Events.OnEnemyDied += (Vector2 enemy_position , int enemy_bullet_cost) => ApplyBaff();
			resetTimer.Connect(Timer.SignalName.Timeout , Callable.From(()=>
			{	
				ResetBaffs();
			}));
			velocityComponent.AddSpeedPercentModifire(SPEED_MODIFIRE , 0);
        }
		private void ApplyBaff()
		{
			resetTimer.Start();
			if(_effectCount < MAX_EFFECT_STACK)
			{
				_effectCount += 1;
				velocityComponent.SetSpeedPercentModifire(SPEED_MODIFIRE , _speedMultiplier * _effectCount);
				hurtBoxComponent.SetArrmor(_arrmorMultiplier *  _effectCount);
			}
			
			
		}
		private void ResetBaffs()
		{
			resetTimer.Stop();
			hurtBoxComponent.SetArrmor(hurtBoxComponent.Armmor - (_arrmorMultiplier * _effectCount));
			velocityComponent.SetSpeedPercentModifire(SPEED_MODIFIRE , 0 );
			_effectCount = 0;
			
			
		}

    }

}
