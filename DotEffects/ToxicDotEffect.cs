using Game.Components;
using Godot;
using System;


namespace DotEffects
{
	
	public partial class ToxicDotEffect : BaseEffect 
	{
		[Export] TimerControllerComponent timer;
		Timer total_effect_duration_timer;
		Timer tick_effect_duration_timer;
        private const float EFFECT_TICK_DURATION = 0.3f;
		private const float EFFECT_TOTAL_DURATION = 10f;
		private float  EFFECT_TICK_DMG = 2f;
		StatusEfffectData _data;
        public override void _Ready()
        {
			total_effect_duration_timer= timer.CreateTimer(OneShoot: true);
			tick_effect_duration_timer = timer.CreateTimer(OneShoot : true);
        }
        public override void ApplyEffect(StatusEfffectData _data)
        {
			this._data = _data;
			total_effect_duration_timer.Start(EFFECT_TOTAL_DURATION);
			total_effect_duration_timer.Connect(Timer.SignalName.Timeout, Callable.From(()=> RemoveEffect()));
			tick_effect_duration_timer.Start(EFFECT_TICK_DURATION);
			tick_effect_duration_timer.Connect(Timer.SignalName.Timeout , Callable.From(() => HandleEffect(_data)));

		}
		public void HandleEffect(StatusEfffectData _data)
		{
			
			_data.healthComponent.Damage(EFFECT_TICK_DMG);
			tick_effect_duration_timer.Start(EFFECT_TICK_DURATION);
			
		}
		public void RemoveEffect()
		{
			_data.healthComponent.Damage(EFFECT_TICK_DMG);
			GD.Print(_data.healthComponent.CurrentHealth);
			QueueFree();
		}
     




    }
	
}
