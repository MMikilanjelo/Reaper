using Godot;
using System;


public partial class ItemSpawnerManager : Node
{
	PackedScene ExpirienceScene;
	game_events GameEvents;
	public override void _Ready()
	{
		GameEvents = GetNode<game_events>("/root/GameEvents");
		ExpirienceScene = ResourceLoader.Load<PackedScene>("res://GameObjects/ExpPeals/experience.tscn");
		GameEvents.Connect(game_events.SignalName.OnEnemyDied, Callable.From((Vector2 pos , int enemy_cost_inBullets)=>
			{
				InstantiateExpirianceVial(pos);
			}
			));
		//GameEvents.OnEnemyDied += (Vector2 enemy_died_position , int _ )=> InstantiateExpirianceVial(enemy_died_position);
		
		
	}
	private void InstantiateExpirianceVial(Vector2 position_to_instantiate)
	{
		experience exp = ExpirienceScene.Instantiate() as experience;
		exp.Position = position_to_instantiate;
		GetTree().GetFirstNodeInGroup("ForeGroundLayer")?.AddChild(exp);
	}


}
