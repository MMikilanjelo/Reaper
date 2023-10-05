using Godot;
using System;
using System.Linq;
using Generation.Alghoritms;
using System.Collections.Generic;
using GameLogick;

public partial class UpgradeManager : Node
{
	[Export] Godot.Collections.Array<Upgrade> upgradesPool = new();
	[Export] experience_manager ExperienceManager;
	[Export] PackedScene UpgradeSceenScene;
	private LootTable<Upgrade> upgradeTable = new LootTable<Upgrade>();
	private game_events game_Events;
	
	private readonly Godot.Collections.Dictionary<string , Godot.Collections.Dictionary<Upgrade , int>> current_upgrades = new ();
	public override void _Ready()
	{	
		game_Events = GetNode<game_events>("/root/GameEvents");
		ExperienceManager.Connect(experience_manager.SignalName.LevelUp , new Callable(this , nameof(OnLevelUp)));
		Upgrade first = new Upgrade("hh" , "gl" , "h123");
		Upgrade second = new Upgrade("h123" , "gbrash" , "h000");
		Upgrade secooo = new Upgrade("h1567h" , "reposiory" , "h111111");
		upgradesPool.Add(first);
		upgradesPool.Add(secooo);
		upgradesPool.Add(second);
	}
	private Godot.Collections.Array<Upgrade> pickUpgrades()
	{
		Godot.Collections.Array<Upgrade> chosenUpgrades = new Godot.Collections.Array<Upgrade>();
		Godot.Collections.Array<Upgrade> filtered_upgrades = upgradesPool.Duplicate();
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
		Godot.Collections.Array<Upgrade> chosenUpgrades = pickUpgrades();
		GD.Print(chosenUpgrades.Count + "chosen upgrades count");
		upgradeScreenInstance.SetAbilitiesUpgrades(chosenUpgrades);
		upgradeScreenInstance.Connect(UpgradeScreen.SignalName.UpgradeSelected , new Callable (this , nameof(OnUpgradeSelected)));
	}
	private void OnUpgradeSelected(Upgrade upgrade)
	{
		ApplyUpgrade(upgrade);
	}
	private void ApplyUpgrade(Upgrade chosenUpgrade)
	{
		var hasUpgrade = current_upgrades.ContainsKey(chosenUpgrade.id);
		if(!hasUpgrade)
		{
			current_upgrades.Add(chosenUpgrade.id , new ());	
			current_upgrades[chosenUpgrade.id].Add(chosenUpgrade ,1);
			
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
