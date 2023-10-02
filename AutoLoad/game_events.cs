using Godot;
using System;
using System.Collections.Generic;	

public partial class game_events : Node
{
	[Signal] public delegate void ExperienceVialCollectedEventHandler(int amount);
	[Signal] public delegate void OnAbilityUpgradeAdedEventHandler(PlayerUpgrades addedUpgrade , Godot.Collections.Dictionary<PlayerUpgrades , int> playerUpgrades);
	[Signal] public delegate void OnEnemyDiedEventHandler(Vector2 enemy_died_position , int enemy_bullet_cost);
	[Signal] public delegate void OnPlayerShootEventHandler(int amount);
	[Signal] public delegate void OnRunOutAmmoEventHandler();

	public void On_ExperienceVialCollected(float amount)
	{
		EmitSignal(SignalName.ExperienceVialCollected, amount);
	}
	public void OmAbilityUpgradeAded(PlayerUpgrades addedUpgrade , Godot.Collections.Dictionary<string , Godot.Collections.Dictionary<PlayerUpgrades , int>> playerUpgrades)
	{
		EmitSignal(SignalName.OnAbilityUpgradeAded , addedUpgrade , playerUpgrades);
	}
	public void EmitEnemyDeathSignal(Vector2 enemy_position , int enemy_bullet_cost)
	{
		EmitSignal(SignalName.OnEnemyDied , enemy_position , enemy_bullet_cost);
	}	
	public void EmitPlayerShootSignal(int amount)
	{
		EmitSignal(SignalName.OnPlayerShoot , amount);
	}
	public void EmitRunOutAmoo()
	{
		EmitSignal(SignalName.OnRunOutAmmo);
	}
}
