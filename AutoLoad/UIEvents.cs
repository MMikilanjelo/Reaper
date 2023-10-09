using Godot;
using System;
using Game.Components;
public partial class UIEvents : Node
{
	[Signal] public delegate void ArmorApplyedEventHandler(float amount);
	[Signal] public delegate void GetPlayerStatsEventHandler();
	public PlayerStats playerStats = new PlayerStats();
	
}
public partial class PlayerStats : RefCounted 
{
	
	public  float dmg_reduction;
	public  float speed_Multiplier;
	public float miis_chance;
	
}