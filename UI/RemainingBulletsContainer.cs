using Godot;
using System;


namespace GameUI
{
	public partial class RemainingBulletsContainer : HBoxContainer
	{
		private game_events Game_Events;
		[Export] private Label ammo_count_label;
		private int ammo_count = 10;
		public override void _Ready()
		{
			ammo_count_label.Text = ammo_count.ToString();
			Game_Events = GetNode<game_events>("/root/GameEvents");
			Game_Events.OnPlayerShoot += (int amount) => DecreseBulletsCount(amount);
			Game_Events.OnEnemyDied += (Vector2 _ , int enemy_cost_inBullets)=> IncreaseBulletsCount(enemy_cost_inBullets);
			
		}
		private void IncreaseBulletsCount(int amount)
		{
			ammo_count += amount;
			ammo_count_label.Text = ammo_count.ToString();
		}
		private void DecreseBulletsCount(int amount)
		{
			ammo_count -= amount;
			ammo_count_label.Text = ammo_count.ToString();
			if(ammo_count == 0){
				Game_Events.EmitRunOutAmoo();
			}
		}


	
	}
}

