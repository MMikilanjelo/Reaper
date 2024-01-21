using Game.Components;
using GameUI;
using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class game_events : Node
{
	
	[Signal] public delegate void ExperienceVialCollectedEventHandler(int amount);
	[Signal] public delegate void OnAbilityUpgradeAdedEventHandler(Upgrade addedUpgrade , Godot.Collections.Dictionary<Upgrade , int> playerUpgrades);
	[Signal] public delegate void OnEnemyDiedEventHandler(Vector2 enemy_died_position , int enemy_bullet_cost);
	[Signal] public delegate void OnPlayerShootEventHandler(int amount);
	[Signal] public delegate void OnRunOutAmmoEventHandler(bool _hasAmmmo);
	[Signal] public delegate void OnBulletChestCollectedEventHandler(int bullets_amount);
	[Signal] public delegate void OnEnemyDmgRecivedEventHandler(int dmg);
	[Signal] public delegate void WaveFinishedEventHandler();
	[Signal] public delegate void NewWaveStartedEventHandler();


	public void EmitDmgRecivedByEnemy(int dmg)
	{
		EmitSignal(SignalName.OnEnemyDmgRecived , dmg);
	}
	public void On_ExperienceVialCollected(float amount)
	{
		EmitSignal(SignalName.ExperienceVialCollected, amount);
	}
	public void OmAbilityUpgradeAded(Upgrade addedUpgrade , Godot.Collections.Dictionary<string , Godot.Collections.Dictionary<Upgrade , int>> playerUpgrades)
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
	public void EmitRunOutAmoo(bool _hasAmmmo)
	{
		EmitSignal(SignalName.OnRunOutAmmo , _hasAmmmo);
	}
	public void EmitChestCollection(int bullets_amount)
	{
		EmitSignal(SignalName.OnBulletChestCollected , bullets_amount);
	}
	public void EmitWaveFinishing()
	{
		EmitSignal(SignalName.WaveFinished);
	}
	public void EmitNewWaveStarting()
	{
		EmitSignal(SignalName.NewWaveStarted);
	}
}
