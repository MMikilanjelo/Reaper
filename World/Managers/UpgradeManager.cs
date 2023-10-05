using Godot;
using System;
using System.Linq;
using Generation.Alghoritms;
using System.Collections.Generic;
using GameLogick;

public partial class UpgradeManager : Node
{
	
	private readonly  Godot.Collections.Array<Upgrade> avaible_common_upgrades_Pool = new ();
	private readonly  Godot.Collections.Array<Upgrade> avaible_rare_upgrades_Pool = new ();
	private readonly Godot.Collections.Array<Upgrade> avaible_legendary_upgrades_Pool = new ();

	[Export] experience_manager ExperienceManager;
	[Export] PackedScene UpgradeSceenScene;
	private readonly LootTable<Upgrade> upgradeTable = new LootTable<Upgrade>();
	private readonly LootTable<Godot.Collections.Array<Upgrade>> tier_List = new();
	private game_events game_Events;
	
	private readonly Godot.Collections.Dictionary<string , Godot.Collections.Dictionary<Upgrade , int>> current_upgrades = new ();
	public override void _Ready()
	{	
		game_Events = GetNode<game_events>("/root/GameEvents");
		ExperienceManager.Connect(experience_manager.SignalName.LevelUp , new Callable(this , nameof(OnLevelUp)));
		tier_List.AddItemToTable(avaible_common_upgrades_Pool,10);
		tier_List.AddItemToTable(avaible_rare_upgrades_Pool,5);
		tier_List.AddItemToTable( avaible_legendary_upgrades_Pool,2);

		Upgrade dmg_reduction = ResourceLoader.Load<Upgrade>("res://Resourses/Upgrades/Common/dmg_reduction.tres");
		Upgrade hp_bonus = ResourceLoader.Load<Upgrade>("res://Resourses/Upgrades/Common/hp_bonus.tres");
		Upgrade move_speed_increment = ResourceLoader.Load<Upgrade>("res://Resourses/Upgrades/Common/move_speed_increment.tres");
	
		
		avaible_common_upgrades_Pool.Add(dmg_reduction);
		avaible_common_upgrades_Pool.Add(hp_bonus);
		avaible_common_upgrades_Pool.Add(move_speed_increment);

		Upgrade armor_reduction = ResourceLoader.Load<Upgrade>("res://Resourses/Upgrades/UnCommon/arrmor_reduction.tres");
		Upgrade miss = ResourceLoader.Load<Upgrade>("res://Resourses/Upgrades/UnCommon/miss_chance.tres");
		Upgrade vampire = ResourceLoader.Load<Upgrade>("res://Resourses/Upgrades/UnCommon/vampire.tres");
		

		avaible_rare_upgrades_Pool.Add(armor_reduction);
		avaible_rare_upgrades_Pool.Add(miss);
		avaible_rare_upgrades_Pool.Add(vampire);

		
		Upgrade serial = ResourceLoader.Load<Upgrade>("res://Resourses/Upgrades/Legendary/Shroud.tres");
		Upgrade toxic = ResourceLoader.Load<Upgrade>("res://Resourses/Upgrades/Legendary/Toxic.tres");
		Upgrade shroud = ResourceLoader.Load<Upgrade>("res://Resourses/Upgrades/Legendary/Unstopoble.tres");
	

		avaible_legendary_upgrades_Pool.Add(serial);
		avaible_legendary_upgrades_Pool.Add(toxic);
		avaible_legendary_upgrades_Pool.Add(shroud);

}
	private Godot.Collections.Array<Upgrade> pickUpgrades(Godot.Collections.Array<Upgrade> upgrades_pool)
	{
		Godot.Collections.Array<Upgrade> chosenUpgrades = new Godot.Collections.Array<Upgrade>();
		Godot.Collections.Array<Upgrade> filtered_upgrades = upgrades_pool.Duplicate();
		for(int i = 0 ; i < 3 ; i ++)
		{
			var chosenUpgrade = filtered_upgrades.PickRandom() as Upgrade;
			chosenUpgrades.Add(chosenUpgrade);
			filtered_upgrades = filterArray(filtered_upgrades , chosenUpgrade);
		}
		return chosenUpgrades;
	}
	private void OnLevelUp(int currentlvl)
	{
		
		var upgradeScreenInstance = UpgradeSceenScene.Instantiate() as UpgradeScreen;
		AddChild(upgradeScreenInstance);
		var current_item_tier = tier_List.PickItem();
		Godot.Collections.Array<Upgrade> chosenUpgrades = pickUpgrades( current_item_tier);
		upgradeScreenInstance.SetAbilitiesUpgrades(chosenUpgrades);
		upgradeScreenInstance.UpgradeSelected += (Upgrade upgrade) => OnUpgradeSelected(upgrade  , current_item_tier);

    }
	
	private void OnUpgradeSelected(Upgrade upgrade ,Godot.Collections.Array<Upgrade> chosen_upgrade_pool )
	{
		ApplyUpgrade(upgrade , chosen_upgrade_pool);
	}
	private void ApplyUpgrade(Upgrade chosenUpgrade , Godot.Collections.Array<Upgrade> chosen_upgrade_pool)
	{
		var hasUpgrade = current_upgrades.ContainsKey(chosenUpgrade.id);
		if(!hasUpgrade)
		{
			
			current_upgrades.Add(chosenUpgrade.id , new ());	
			current_upgrades[chosenUpgrade.id].Add(chosenUpgrade ,1);
			if(chosenUpgrade.isUnique)
			{
				chosen_upgrade_pool.Remove(chosenUpgrade);
				
			}
			
			
		}
		else 
		{
			current_upgrades[chosenUpgrade.id][chosenUpgrade] += 1; 
		}
		foreach(KeyValuePair<Upgrade , int> pair in current_upgrades[chosenUpgrade.id] )
		{
		 	GD.Print(pair.Key , pair.Value);
		}
		
		game_Events.OmAbilityUpgradeAded(chosenUpgrade , current_upgrades);
	}
	private Godot.Collections.Array<Upgrade> filterArray(Godot.Collections.Array<Upgrade> arrayToFilter , Upgrade chosenUpgrade)
	{
		Godot.Collections.Array<Upgrade> filteredArray = new();
		foreach(var upgrade in arrayToFilter)
		{
			if(upgrade.id != chosenUpgrade.id)
			{
				filteredArray.Add(upgrade);
			}
		}
		return filteredArray;
	}
	private Upgrade GetRandomUpgrade(Godot.Collections.Array<Upgrade> upgrades)
	{
		return  upgrades[Directions.random.RandiRange(0 , upgrades.Count - 1)];
	}
	private Godot.Collections.Array<Upgrade> GetUpgradeFromWeigthTable()
	{
		return new Godot.Collections.Array<Upgrade>();
	}
}
