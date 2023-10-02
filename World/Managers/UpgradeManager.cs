using Godot;
using System;
using System.Linq;
using Generation.Alghoritms;
using System.Collections.Generic;

public partial class UpgradeManager : Node
{
	[Export] Godot.Collections.Array<PlayerUpgrades> upgradesPool = new();
	[Export] experience_manager ExperienceManager;
	[Export] PackedScene UpgradeSceenScene;
	private game_events game_Events;
	private readonly Godot.Collections.Dictionary<string , Godot.Collections.Dictionary<PlayerUpgrades , int>> current_upgrades = new ();
	public override void _Ready()
	{	
		game_Events = GetNode<game_events>("/root/GameEvents");
		ExperienceManager.Connect(experience_manager.SignalName.LevelUp , new Callable(this , nameof(OnLevelUp)));
		

	}
	private Godot.Collections.Array<PlayerUpgrades> pickUpgrades()
	{
		Godot.Collections.Array<PlayerUpgrades> chosenUpgrades = new Godot.Collections.Array<PlayerUpgrades>();
		Godot.Collections.Array<PlayerUpgrades> filtered_upgrades = upgradesPool.Duplicate();
		for(int i = 0 ; i < 3 ; i ++)
		{
			var chosenUpgrade = filtered_upgrades.PickRandom() as PlayerUpgrades;
			chosenUpgrades.Add(chosenUpgrade);
			filtered_upgrades = filterArray(filtered_upgrades , chosenUpgrade);
		}
		return chosenUpgrades;
	}
	private void OnLevelUp(int currentlvl)
	{
		
		var upgradeScreenInstance = UpgradeSceenScene.Instantiate() as UpgradeScreen;
		AddChild(upgradeScreenInstance);
		Godot.Collections.Array<PlayerUpgrades> chosenUpgrades = pickUpgrades();
		GD.Print(chosenUpgrades.Count + "chosen upgrades count");
		upgradeScreenInstance.SetAbilitiesUpgrades(chosenUpgrades);
		upgradeScreenInstance.Connect(UpgradeScreen.SignalName.UpgradeSelected , new Callable (this , nameof(OnUpgradeSelected)));
	}
	private void OnUpgradeSelected(PlayerUpgrades upgrade)
	{
		ApplyUpgrade(upgrade);
	}
	private void ApplyUpgrade(PlayerUpgrades chosenUpgrade)
	{
		var hasUpgrade = current_upgrades.ContainsKey(chosenUpgrade.id);
		if(!hasUpgrade)
		{
			current_upgrades.Add(chosenUpgrade.id , new ());	
			current_upgrades[chosenUpgrade.id].Add(chosenUpgrade ,1 );
			
		}
		else 
		{
			current_upgrades[chosenUpgrade.id][chosenUpgrade] += 1; 
		}
		foreach(KeyValuePair<PlayerUpgrades , int> pair in current_upgrades[chosenUpgrade.id] )
		{
		 	GD.Print(pair.Key , pair.Value);
		}
		game_Events.OmAbilityUpgradeAded(chosenUpgrade , current_upgrades);
	}
	private Godot.Collections.Array<PlayerUpgrades> filterArray(Godot.Collections.Array<PlayerUpgrades> arrayToFilter , PlayerUpgrades chosenUpgrade)
	{
		Godot.Collections.Array<PlayerUpgrades> filteredArray = new();
		foreach(var upgrade in arrayToFilter)
		{
			if(upgrade.id != chosenUpgrade.id)
			{
				filteredArray.Add(upgrade);
			}
		}
		return filteredArray;
	}
	private PlayerUpgrades GetRandomUpgrade(Godot.Collections.Array<PlayerUpgrades> upgrades)
	{
		return  upgrades[Directions.random.RandiRange(0 , upgrades.Count - 1)];
	}

}
