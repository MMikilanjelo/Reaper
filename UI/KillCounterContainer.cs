using Godot;
using System;


namespace GameUI
{
	public partial class KillCounterContainer : HBoxContainer
	{
		[Export] private Label kill_count_label;
		private int currentKills = 0;
		private game_events Game_Events;
        public override void _Ready()
        {
			
			Game_Events = GetNode<game_events>("/root/GameEvents");
			Game_Events.OnEnemyDied += (Vector2 enemy_died_position , int _)=> UpdateKillsCount();
			kill_count_label.Text = currentKills.ToString();
        }
        public override void _Process(double delta)
        {
            base._Process(delta);
        }
		private void UpdateKillsCount()
		{
			currentKills++;
			kill_count_label.Text = currentKills.ToString();
		}
    }
}

