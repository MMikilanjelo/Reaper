using Godot;
using System;
using GameLogick.Utilities;
using PlayerPassive;
using System.Reflection.Metadata;

namespace Game.Components
{
	public partial class AbilitieUpgradeRecivierComponent : Node
	{
		[Export] ResourcePreloader resourcePreloader;
		game_events Game_Events;
		PlayerController player;
		HurtBoxComponent playerHurtBox;
		WeaponRootComponent playerWeaponRootComponent;
		UIEvents uiEvents;
		PackedScene lifeSteal;
		PackedScene shroudPassive;
		PackedScene unstopoble;
        public override void _Ready()
        {
			Game_Events = GetNode<game_events>("/root/GameEvents");
			uiEvents = GetNode<UIEvents>("/root/UIEvents");
			player  = GetParent<PlayerController>();
			playerHurtBox = player.GetNode<HurtBoxComponent>("HurtBoxComponent");
			playerWeaponRootComponent = player.GetNode<WeaponRootComponent>("Visuals/CanvasGroup/RotationPivot/WeaponRootComponent");
			
			
			Game_Events.Connect(game_events.SignalName.OnAbilityUpgradeAded , new Callable(this , nameof(OnAbilityUpgradeAded)));
			
			lifeSteal = resourcePreloader.GetResource("LifeStealPassive") as PackedScene;
			shroudPassive = resourcePreloader.GetResource("ShroudPassive") as PackedScene;
			unstopoble = resourcePreloader.GetResource("UnstopablePassive") as PackedScene;
        }
		private void OnAbilityUpgradeAded(Upgrade addedUpgrade, Godot.Collections.Dictionary<string, Godot.Collections.Dictionary<Upgrade, int>> currentPlayerUpgrades)
		{
			switch (addedUpgrade.id)
			{
				case "hp_bonus":
					player.healthComponent.IncreaseMaxHealth(10);
					break;

				case "move_speed":
					player.velocityComponent.SpeedMultiplier += addedUpgrade.value;
					uiEvents.playerStats.speed_Multiplier = player.velocityComponent.SpeedMultiplier;
					break;

				case "dmg_reduction":
					playerHurtBox.SetDmgReductonMultiplier(addedUpgrade.value);
					uiEvents.playerStats.dmg_reduction = playerHurtBox.DmgReductonMultiplier;
					break;

				case "miss_chance":
					playerHurtBox.SetMissChance(addedUpgrade.value);
					uiEvents.playerStats.miis_chance = playerHurtBox.MissChance;
					break;

				case "toxic":
					playerWeaponRootComponent.AddAfexToWeapon(ResourceLoader.Load<PackedScene>("res://DotEffects/ToxicEffect/ToxicDotEffect.tscn"));
					break;

				case "vampire":
					var vampireAbility = lifeSteal.Instantiate() as LifeStealPassive;
					vampireAbility.healthComponent = player.GetNode<HealthComponent>("HealthComponent");
					player.AddChild(vampireAbility);
					break;
				case "shroud" :
					var shroudAbility = shroudPassive.Instantiate() as ShroudPassive;
					shroudAbility.hurtBoxComponent = playerHurtBox;
					player.AddChild(shroudAbility);
					break;
				case "unstopoble":
					var unstopobleAbility = unstopoble.Instantiate() as UnstopablePassive;
					unstopobleAbility.velocityComponent = player.velocityComponent;
					unstopobleAbility.hurtBoxComponent = playerHurtBox;
					player.AddChild(unstopobleAbility);
					break;
				case "arrmor":
					playerHurtBox.IncreaseArrmor((int)addedUpgrade.value);
					break;

			}
		}
	}
	
}

