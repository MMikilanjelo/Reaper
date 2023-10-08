using Godot;
using System;
using GameLogick.Utilities;

namespace Game.Components
{
	public partial class AbilitieUpgradeRecivierComponent : Node
	{
		game_events Game_Events;
		PlayerController player;
		HurtBoxComponent playerHurtBox;
        public override void _Ready()
        {
			player  = GetParent<PlayerController>();
			playerHurtBox = player.GetNode<HurtBoxComponent>("HurtBoxComponent");
			Game_Events = GetNode<game_events>("/root/GameEvents");
			Game_Events.Connect(game_events.SignalName.OnAbilityUpgradeAded , new Callable(this , nameof(OnAbilityUpgradeAded)));
        }
		private void OnAbilityUpgradeAded(Upgrade addedUpgrade ,  Godot.Collections.Dictionary<string , Godot.Collections.Dictionary<Upgrade , int>> currentPlayerUpgrades)
		{
			if(addedUpgrade.id == "hp_bonus")
			{
				player.healthComponent.SetCurrentHealth(10);
			}
			if(addedUpgrade.id == "move_speed")
			{
				player.velocityComponent.SpeedMultiplier += 0.1f;
			}
			if(addedUpgrade.id =="dmg_reduction")
			{
				playerHurtBox.SetDmgReductonMultiplier(0.1f);
			}
			if(addedUpgrade.id == "miss_chance")
			{
				playerHurtBox.SetMissChance(100);
			}	
		}
    }
}

