using Godot;
using System;

public partial class experience_manager : Node
{
	const int TARGET_EXP_GROWTH  = 1;
	game_events game_Events;
	int current_Experience = 0;
	int current_level = 1;
	int target_exp = 1;
	
	[Signal] public delegate void ExperienceCollectedEventHandler(int current_exp , int target_exp);
	[Signal] public delegate void LevelUpEventHandler(int newLvl);
	
	
	
	public override void _Ready()
	{
		game_Events = GetNode<game_events>("/root/GameEvents");
		game_Events.Connect(game_events.SignalName.ExperienceVialCollected ,  new Callable(this , nameof(onExperienceVialCollected)));
	}
	private void onExperienceVialCollected(int amout)
	{
		increment_expereince(amout);
	}
	private void increment_expereince(int amount)
	{
		current_Experience = Math.Min(current_Experience + amount , target_exp);
		EmitSignal(SignalName.ExperienceCollected , current_Experience , target_exp);
		if(current_Experience == target_exp)
		{
			current_level += 1 ;
			target_exp += 	TARGET_EXP_GROWTH;
			current_Experience = 0;
			EmitSignal(SignalName.ExperienceCollected , current_Experience , target_exp);
			EmitSignal(SignalName.LevelUp , current_level);
		}
	}
}
