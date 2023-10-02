using Godot;
using System;

public partial class ExperienceBar : CanvasLayer
{
	[Export] experience_manager Experience_Manager;
	[Export] ProgressBar progressBar;

	public override void _Ready()
	{
		progressBar.Value = 0;
		Experience_Manager.Connect(experience_manager.SignalName.ExperienceCollected , new Callable(this , nameof(OnExperienceUpdated)));


	}
	private void OnExperienceUpdated(int current_exp , int target_exp)
	{
		progressBar.MaxValue = target_exp;
		progressBar.Value = current_exp;
	}
}
