using Game.Components;
using GameUI;
using Godot;
using System;


namespace DotEffects
{
	
	public partial class ToxicDotEffect : BaseEffect 
	{
		[Export] TimerControllerComponent timer;
		[Export] PackedScene EffectPatrickle;
		PackedScene floatingTextScene;
		Timer total_effect_duration_timer;
		Timer tick_effect_duration_timer;
		
		private  int total_count_of_effect;
        public override void _Ready()
        {
			effectStatsData = ResourceLoader.Load<EffectStats>("res://DotEffects/ToxicEffect/ToxicEfectData.tres");
			total_count_of_effect = (int)(effectStatsData.EFFECT_TOTAL_DURATION/effectStatsData.EFFECT_TICK_DURATION);
			floatingTextScene = ResourceLoader.Load("res://UI/FloatingText.tscn") as PackedScene;
			
			total_effect_duration_timer= timer.CreateTimer(OneShoot: true);
			tick_effect_duration_timer = timer.CreateTimer(OneShoot : true);
        }
        public override void ApplyEffect(StatusEfffectData _data)
        {

			total_effect_duration_timer.Start(effectStatsData.EFFECT_TOTAL_DURATION);
			total_effect_duration_timer.Connect(Timer.SignalName.Timeout, Callable.From(()=> RemoveEffect(_data)));
			tick_effect_duration_timer.Start(effectStatsData.EFFECT_TICK_DURATION);
			tick_effect_duration_timer.Connect(Timer.SignalName.Timeout , Callable.From(() => HandleEffect(_data)));
		}
		public void HandleEffect(StatusEfffectData _data)
		{
			if(_data == null) {
				return ;
			}
			
			total_count_of_effect --;
			_data?.healthComponent?.Damage(effectStatsData.EFFECT_TICK_DMG);
			//AddFloatingText(floatingTextScene , GlobalPosition , effectStatsData.EFFECT_TICK_DMG.ToString());
			AddPartcike(EffectPatrickle);
			tick_effect_duration_timer.Start(effectStatsData.EFFECT_TICK_DURATION);
			
		}
		public void RemoveEffect(StatusEfffectData _data)
		{
			
			_data.healthComponent.Damage(effectStatsData.EFFECT_TICK_DMG);
			
			//AddFloatingText(floatingTextScene , GlobalPosition , effectStatsData.EFFECT_TICK_DMG.ToString());
			AddPartcike(EffectPatrickle);
			EmitSignal(SignalName.OnRemoveEfect);
			QueueFree();
		}
		private void AddFloatingText(PackedScene floating_text_scene, Vector2 position , string text)
		{
		 	var floatText =	floating_text_scene.Instantiate() as FloatingText;
			GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild( floatText);
			floatText.GlobalPosition = position;
			floatText.SetScale(0.4f);
			floatText.Start(text);
			
		}
		private void AddPartcike(PackedScene partickle)
		{
			GpuParticles2D 	patickles = partickle.Instantiate() as GpuParticles2D;
			patickles.Position = Position;
			AddChild(patickles);
		}
	




    }
	
}
