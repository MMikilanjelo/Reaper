using Godot;
using System;

namespace GameUI
{
	
	public partial class FloatingText : Node2D
	{
		[Export] private Label floating_text_label;
		private float TextScaleMultiplier = 1.3f;
		private Vector2 BaseScale = Vector2.One;
		public void Start(string text)
		{
			floating_text_label.Text = text;
			Tween tween = CreateTween();
			tween.TweenProperty(this , "position" , GlobalPosition + (Vector2.Up* 16) , .15f ).
			SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
			
			tween.Chain();
			tween.TweenProperty(this  ,"position" ,  GlobalPosition + (Vector2.Up* 32) , .3f)
			.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
			tween.TweenProperty(this , "scale" , Vector2.Zero  , .3f)
			.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
			tween.Chain();

			tween.TweenCallback(Callable.From(()=>QueueFree()));

			Tween scale_tween = CreateTween();
			scale_tween.TweenProperty(this , "scale" , Vector2.One * TextScaleMultiplier  , .15f)
			.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
			scale_tween.TweenProperty(this , "scale" , BaseScale , .15f)
			.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
		}
		public void SetScale(float newScaleMultiplier)
		{
			TextScaleMultiplier = newScaleMultiplier;
			BaseScale *= newScaleMultiplier;
			
		}
    }
}

