using Game.Components;
using Godot;
using System;


namespace DotEffects
{
	
	public partial class ToxicDotEffect : BaseEffect 
	{
		[Export] TimerControllerComponent timer;
		Timer total_effect_duration_timer;
        private const float EFFECT_TICK_DURATION = 0.1f;
		private const float EFFECT_TOTAL_DURATION = 3f;
		private float  EFFECT_TICK_DMG = 1f;
        public override void _Ready()
        {
			total_effect_duration_timer= timer.CreateTimer(OneShoot: true);
        }
        public override void ApplyEffect()
        {
			GD.Print("I am toxic effect");
			total_effect_duration_timer.Start(EFFECT_TOTAL_DURATION);
			GD.Print(total_effect_duration_timer.TimeLeft);
		}
       


    }
	
}
