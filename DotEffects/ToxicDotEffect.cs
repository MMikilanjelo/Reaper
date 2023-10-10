using Game.Components;
using GameUI;
using Godot;
using System;


namespace DotEffects
{
	
	public partial class ToxicDotEffect : BaseEffect 
	{
		PackedScene floatingTextScene;
		[Export] TimerControllerComponent timer;
		Label poisonCountersLabel;
		Timer total_effect_duration_timer;
		Timer tick_effect_duration_timer;
		
        private const float EFFECT_TICK_DURATION = 1f;
		private const float EFFECT_TOTAL_DURATION = 5f;
		private const float  EFFECT_TICK_DMG = 10f;
		private  int total_count_of_effect = (int)(EFFECT_TOTAL_DURATION/EFFECT_TICK_DURATION);
        public override void _Ready()
        {
			floatingTextScene = ResourceLoader.Load("res://UI/FloatingText.tscn") as PackedScene;
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
			if(_data == null) {
				return ;
			}
			total_count_of_effect --;
			_data?.healthComponent?.Damage(EFFECT_TICK_DMG);
			
			//AddFloatingText(floatingTextScene , GlobalPosition , EFFECT_TICK_DMG.ToString());
			poisonCountersLabel.Text = total_count_of_effect.ToString();
			tick_effect_duration_timer.Start(EFFECT_TICK_DURATION);
			
		}
		public void RemoveEffect(StatusEfffectData _data)
		{
			_data.healthComponent.Damage(EFFECT_TICK_DMG);
			//AddFloatingText(floatingTextScene , GlobalPosition , EFFECT_TICK_DMG.ToString());
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




    }
	
}
