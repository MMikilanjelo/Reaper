using Godot;
using System;
using Game.Components;
using Game.Enteties;
public partial class UIEvents : Node
{
	[Signal] public delegate void ArmorApplyedEventHandler(float amount);
	[Signal] public delegate void GetPlayerStatsEventHandler();
	[Signal] public delegate void PlayerCurrencyUpdatedEventHandler(int amount);
	public PlayerStats playerStats = new PlayerStats();
	public PlayerStatistic playerStatistic = new PlayerStatistic();
	private game_events Game_Events;
    public override void _Ready()
    {
        Game_Events = GetNode<game_events>("/root/GameEvents");
		Game_Events.Connect(game_events.SignalName.OnEnemyDied, 
		Callable.From((Vector2 pos , int enemy_cost_inBullets) => 
		{
			playerStatistic._totalKills++;
			playerStatistic._currentCurrency ++;
			EmitSignal(SignalName.PlayerCurrencyUpdated , playerStatistic._currentCurrency);
		}));
		Game_Events.Connect(game_events.SignalName.OnAbilityUpgradeAded, 
		Callable.From((Upgrade addedUpgrade , Godot.Collections.Dictionary<Upgrade , int> playerUpgrades) => 
		{
			playerStatistic._totalUpgrades++;
		}));
		Game_Events.Connect(game_events.SignalName.OnEnemyDmgRecived, 
		Callable.From((int dmg , EnemyData _enemyData) => 
		{
			playerStatistic._totalDealtDmg += dmg;
		}));
		Game_Events.Connect(game_events.SignalName.ExperienceVialCollected, 
		Callable.From((int amount) => 
		{
			playerStatistic._totalSouls += amount;
		}));
		Game_Events.Connect(game_events.SignalName.ShopSlotPurchased , Callable.From((ShopSlotData _itemData)=>
		{
			playerStatistic._currentCurrency -= (int)_itemData._itemCost;
			EmitSignal(SignalName.PlayerCurrencyUpdated , playerStatistic._currentCurrency);
		}));
    }
	public void ResetStatistik()
	{
		playerStatistic._totalDealtDmg = 0;
		playerStatistic._totalKills = 0;
		playerStatistic._totalUpgrades = 0;
		playerStatistic._totalSouls = 0;
		playerStatistic._currentCurrency = 0;
	}

}
public partial class PlayerStats : RefCounted 
{
	public  float dmg_reduction;
	public  float speed_Multiplier;
	public float miis_chance;
	
}
public partial class PlayerStatistic : RefCounted
{
	public int _totalKills = 0;
    public int _totalUpgrades = 0;
    public int _totalDealtDmg = 0;
    public int _totalSouls = 0;
	public int _currentCurrency = 0;
}