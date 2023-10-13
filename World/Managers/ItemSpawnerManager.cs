using Godot;
using System;


public partial class ItemSpawnerManager : Node
{
	[Export] PackedScene ExpirienceScene;
	game_events GameEvents;
	public override void _Ready()
	{
		GameEvents = GetNode<game_events>("/root/GameEvents");

		GameEvents.OnEnemyDied += (Vector2 enemy_died_position , int _ )=> InstantiateExpirianceVial(enemy_died_position);
		
		
	}
	private void InstantiateExpirianceVial(Vector2 position_to_instantiate)
	{
		experience exp = ExpirienceScene.Instantiate() as experience;
		exp.Position = position_to_instantiate;
		GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(exp);
	}


}
