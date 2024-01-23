using Godot;
using System;


namespace GameUI
{
	public partial class KillCounterContainer : HBoxContainer
	{
		[Export] private Label kill_count_label;
		private int currentKills = 0;
		private game_events Game_Events;
		private UIEvents _uiEvents;
        public override void _Ready()
        {
			_uiEvents = GetNode<UIEvents>("/root/UIEvents");
			Game_Events = GetNode<game_events>("/root/GameEvents");
			Game_Events.Connect(game_events.SignalName.OnEnemyDied, Callable.From((Vector2 pos , int enemy_cost_inBullets)=>
			{
				UpdateKillsCount();
			}
			));
			kill_count_label.Text = currentKills.ToString();
        }
		private void UpdateKillsCount()
		{
			currentKills++;
			kill_count_label.Text = currentKills.ToString();
			_uiEvents.playerStatistic._currentCurrency ++;
		}
        public override void _ExitTree()
        {
			
        }
    }
}

