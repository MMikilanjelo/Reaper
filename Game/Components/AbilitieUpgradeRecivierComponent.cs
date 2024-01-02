using Godot;
using System;
using GameLogick.Utilities;
using PlayerPassive;

namespace Game.Components
{
	public partial class AbilitieUpgradeRecivierComponent : Node
	{
		game_events Game_Events;
		PlayerController player;
		HurtBoxComponent playerHurtBox;
		WeaponRootComponent playerWeaponRootComponent;
		UIEvents uiEvents;
		PackedScene lifeSteal;
        public override void _Ready()
        {
			uiEvents = GetNode<UIEvents>("/root/UIEvents");
			player  = GetParent<PlayerController>();
			playerHurtBox = player.GetNode<HurtBoxComponent>("HurtBoxComponent");
			playerWeaponRootComponent = player.GetNode<WeaponRootComponent>("Visuals/CanvasGroup/RotationPivot/WeaponRootComponent");
			Game_Events = GetNode<game_events>("/root/GameEvents");
			Game_Events.Connect(game_events.SignalName.OnAbilityUpgradeAded , new Callable(this , nameof(OnAbilityUpgradeAded)));

			lifeSteal = ResourceLoader.Load("res://PlayerPassive/LifeStealPassive.tscn") as PackedScene;
        }
		private void OnAbilityUpgradeAded(Upgrade addedUpgrade ,  Godot.Collections.Dictionary<string , Godot.Collections.Dictionary<Upgrade , int>> currentPlayerUpgrades)
		{
			if(addedUpgrade.id == "hp_bonus")
			{
				player.healthComponent.SetCurrentHealth(10);
			}
			if(addedUpgrade.id == "move_speed")
			{
				player.velocityComponent.SpeedMultiplier += addedUpgrade.value;
				uiEvents.playerStats.speed_Multiplier = player.velocityComponent.SpeedMultiplier;
			}
			if(addedUpgrade.id =="dmg_reduction")
			{
				playerHurtBox.SetDmgReductonMultiplier(addedUpgrade.value);
				uiEvents.playerStats.dmg_reduction = playerHurtBox.DmgReductonMultiplier;
            }
			if(addedUpgrade.id == "miss_chance")
			{
				playerHurtBox.SetMissChance(addedUpgrade.value);
				uiEvents.playerStats.miis_chance = playerHurtBox.MissChance;
			}	
			if(addedUpgrade.id == "toxic")
			{
				playerWeaponRootComponent.AddAfexToWeapon(ResourceLoader.Load<PackedScene>("res://DotEffects/ToxicEffect/ToxicDotEffect.tscn"));	
			}
			if(addedUpgrade.id == "vampire")
			{
				var vampireAbility = lifeSteal.Instantiate() as LifeStealPassive;
				vampireAbility.healthComponent = player.GetNode<HealthComponent>("HealthComponent"); 
				player.AddChild(vampireAbility);
			}
		}
	
    }
	
}

