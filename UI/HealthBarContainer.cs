using Game.Components;
using Godot;
using System;


namespace GameUI
{
	public partial class HealthBarContainer : VBoxContainer
	{
		[Export] TextureProgressBar playerHealth;
		
		public override void _Ready()
		{
			var Player = GetTree().GetFirstNodeInGroup("Player") as PlayerController;
			 
			
		    Player.GetNode<HealthComponent>("HealthComponent").Connect(HealthComponent.SignalName.HealthChanged , Callable.From((HealthComponent.HealthUpdate healthUpdate)=>
				UpdatePlayerHealth(healthUpdate)
					
			));
		}

		private void UpdatePlayerHealth(HealthComponent.HealthUpdate healthUpdate)
		{
			playerHealth.MaxValue = healthUpdate.MaxHealth;
			playerHealth.Value = healthUpdate.CurrentHealth;
		}
	}

}
