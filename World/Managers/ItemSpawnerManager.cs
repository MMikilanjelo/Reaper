using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class ItemSpawnerManager : Node
{
	PackedScene ExpirienceScene;
	PackedScene ChestScene;
	PackedScene _shopScene;
	game_events _gameEvents;
	[Export] Timer timer;
	public static  RandomNumberGenerator random = new RandomNumberGenerator();
	const float SPAWN_RADIUS = 400f;
	public override void _Ready()
	{
		GetReferences();
		ConnectToSignals();
	}
	private void GetReferences()
	{
		//Nodes 
		_gameEvents = GetNode<game_events>("/root/GameEvents");
		// Scenes
		ExpirienceScene = ResourceLoader.Load<PackedScene>("res://GameObjects/ExpPeals/experience.tscn");
		ChestScene = ResourceLoader.Load<PackedScene>("res://GameObjects/Chest/chest.tscn");
		_shopScene = ResourceLoader.Load<PackedScene>("res://Shop/Shop.tscn");
	}
	private void ConnectToSignals()
	{
		_gameEvents.Connect(game_events.SignalName.OnEnemyDied, Callable.From((Vector2 pos , int enemy_cost_inBullets)=>InstantiateExpirianceVial (pos)));
		_gameEvents.Connect(game_events.SignalName.WaveFinished , Callable.From( ()=> SpawnShop(Vector2.Zero)));
		timer.Connect(Timer.SignalName.Timeout , Callable.From(() =>  SpawnBulletChest(GetSpawnPosition())));
	}
	private Vector2 GetSpawnPosition()
	{
		var player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody2D;
		if(player == null)
		{
			return Vector2.Zero;
		}
		var randomDirection = Vector2.Right.Rotated(random.RandfRange(0 , MathF.Tau));
		for(int i = 0 ; i < 4 ; i++)
		{
				
			var spawnPosition = player.GlobalPosition + (randomDirection * SPAWN_RADIUS );
			var addditional_check_offset = randomDirection * 2;
			var quety_parameters = PhysicsRayQueryParameters2D.Create(player.GlobalPosition , spawnPosition  + addditional_check_offset , 1);
			var result =  GetTree().Root.World2D.DirectSpaceState.IntersectRay(quety_parameters);
			if (result.Count == 0)
			{
				return spawnPosition ;
			}
			else
			{
				randomDirection = randomDirection.Rotated(Godot.Mathf.DegToRad(90));
			}	
		}
		return Vector2.Zero;
	}
	private void InstantiateExpirianceVial(Vector2 position_to_instantiate)
	{
		experience exp = ExpirienceScene.Instantiate() as experience;
		exp.Position = position_to_instantiate;
		GetTree().GetFirstNodeInGroup("ForeGroundLayer")?.AddChild(exp);
	}
	private void SpawnBulletChest(Vector2 position_to_spawn)
	{
		Chest chest = ChestScene.Instantiate() as Chest;
		chest.Position = position_to_spawn;
		GetTree().GetFirstNodeInGroup("ForeGroundLayer")?.AddChild(chest);
	}
	private void SpawnShop(Vector2 _positionToSpawn)
	{
		var _shopInstance = _shopScene.Instantiate() as Shop;
		GetTree().GetFirstNodeInGroup("ForeGroundLayer")?.AddChild(_shopInstance);
		_shopInstance.GlobalPosition = _positionToSpawn;
	}


}
