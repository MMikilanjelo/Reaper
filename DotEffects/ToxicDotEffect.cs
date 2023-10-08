using Game.Components;
using Godot;
using System;


namespace DotEffects
{
	
	public partial class ToxicDotEffect : BaseEffect 
	{
		[Export] TimerControllerComponent timer;
		Label poisonCountersLabel;
		Timer total_effect_duration_timer;
		Timer tick_effect_duration_timer;
        private const float EFFECT_TICK_DURATION = 0.5f;
		private const float EFFECT_TOTAL_DURATION = 5f;
		private float  EFFECT_TICK_DMG = 10f;
		private  int total_count_of_effect = (int)(EFFECT_TOTAL_DURATION/EFFECT_TICK_DURATION);
        public override void _Ready()
        {
			poisonCountersLabel = GetNode<Label>("VBoxContainer/HBoxContainer/Label");
			total_effect_duration_timer= timer.CreateTimer(OneShoot: true);
			tick_effect_duration_timer = timer.CreateTimer(OneShoot : true);
        }
        public override void ApplyEffect(StatusEfffectData _data)
        {
			poisonCountersLabel.Text = total_count_of_effect.ToString();
			total_effect_duration_timer.Start(EFFECT_TOTAL_DURATION);
			total_effect_duration_timer.Connect(Timer.SignalName.Timeout, Callable.From(()=> RemoveEffect(_data)));
			tick_effect_duration_timer.Start(EFFECT_TICK_DURATION);
			tick_effect_duration_timer.Connect(Timer.SignalName.Timeout , Callable.From(() => HandleEffect(_data)));

		}
		public void HandleEffect(StatusEfffectData _data)
		{
			total_count_of_effect --;
			_data.healthComponent.Damage(EFFECT_TICK_DMG);
			poisonCountersLabel.Text = total_count_of_effect.ToString();
			tick_effect_duration_timer.Start(EFFECT_TICK_DURATION);
			
		}
		public void RemoveEffect(StatusEfffectData _data)
		{
			_data.healthComponent.Damage(EFFECT_TICK_DMG);
			
			QueueFree();
		}




    }
	
}
