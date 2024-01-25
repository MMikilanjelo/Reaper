using Godot;
using System;
using GameLogick.Utilities;
using PlayerPassive;
using System.Reflection.Metadata;

namespace Game.Components
{
	public partial class AbilitieUpgradeRecivierComponent : Node
	{
		game_events Game_Events;
		PlayerController player;
		HurtBoxComponent playerHurtBox;
		WeaponRootComponent playerWeaponRootComponent;
		UIEvents uiEvents;
        public override void _Ready()
        {
			Game_Events = GetNode<game_events>("/root/GameEvents");
			uiEvents = GetNode<UIEvents>("/root/UIEvents");
			player  = GetParent<PlayerController>();
			playerHurtBox = player.GetNode<HurtBoxComponent>("HurtBoxComponent");
			playerWeaponRootComponent = player.GetNode<WeaponRootComponent>("Visuals/CanvasGroup/RotationPivot/WeaponRootComponent");
			
			
			Game_Events.Connect(game_events.SignalName.OnAbilityUpgradeAded , new Callable(this , nameof(OnAbilityUpgradeAded)));
		}
		private void OnAbilityUpgradeAded(Upgrade addedUpgrade, Godot.Collections.Dictionary<string, Godot.Collections.Dictionary<Upgrade, int>> currentPlayerUpgrades)
		{
			switch (addedUpgrade.id)
			{
				case "hp_bonus":
					player._healthComponent.IncreaseMaxHealth(10);
					break;
				case "move_speed":
					player._velocityComponent.SpeedMultiplier += addedUpgrade.value;
					uiEvents.playerStats.speed_Multiplier = player._velocityComponent.SpeedMultiplier;
					break;
				case "dmg_reduction":
					playerHurtBox.SetDmgReductonMultiplier(addedUpgrade.value);
					uiEvents.playerStats.dmg_reduction = playerHurtBox.DmgReductonMultiplier;
					break;
				case "miss_chance":
					playerHurtBox.SetMissChance(addedUpgrade.value);
					uiEvents.playerStats.miis_chance = playerHurtBox.MissChance;
					break;
				case "arrmor":
					playerHurtBox.IncreaseArrmor((int)addedUpgrade.value);
					break;
					
			}
		}
	}
	
}

