using Godot;
using System;
using GameLogick;
using Game.Enteties;
namespace Managers
{
	public partial class enemy_spawner_manager : Node
	{
		[Export] PackedScene BasickEnemyScene;
		[Export] PackedScene BatEnemyScene;
		[Export] PackedScene KnigthEnemyScene;
		[Export] PackedScene DummyTargetScene;
	  	[Export] PackedScene CactusScene;
		[Export] Timer EnemySpawnerInterval;
		[Export] const float SPAWN_RADIUS = 100f;
		[Export] private bool _deactivate = false;
		CharacterBody2D player;
		game_events _gameEvents;
		public static  RandomNumberGenerator random = new RandomNumberGenerator();
		private readonly LootTable<PackedScene> enemyTable = new LootTable<PackedScene>();
		public override void _Ready()
		{
			_gameEvents = GetNode<game_events>("/root/GameEvents");
			enemyTable.AddItemToTable(KnigthEnemyScene , 100);
			enemyTable.AddItemToTable(BasickEnemyScene , 80);
			enemyTable.AddItemToTable(BatEnemyScene , 60);
     	 	enemyTable.AddItemToTable(CactusScene , 40);
      		enemyTable.AddItemToTable(DummyTargetScene , 10);
			ConnectSignals();
			EnemySpawnerInterval.Connect(Timer.SignalName.Timeout , new Callable(this, nameof(SpawnEnemy)));
		}
		private void ConnectSignals()
		{
			_gameEvents.Connect(game_events.SignalName.WaveFinished , Callable.From(()=>
			{
				_deactivate = true;	
			}));
			_gameEvents.Connect(game_events.SignalName.NewWaveStarted , Callable.From(()=>
			{
				_deactivate = false;	
			}));
		}
		private Vector2 GetSpawnPosition()
		{
			player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody2D;
			if(player == null)
			{
				return Vector2.Zero;
			}
			var spawnPosition = Vector2.Zero;
			var randomDirection = Vector2.Right.Rotated(random.RandfRange(0 , MathF.Tau));
			for(int i = 0 ; i < 4 ; i++)
			{
				
				spawnPosition = player.GlobalPosition + (randomDirection * SPAWN_RADIUS );
				var addditional_check_offset = randomDirection * 2;
				var quety_parameters = PhysicsRayQueryParameters2D.Create(player.GlobalPosition , spawnPosition  + addditional_check_offset , 1);
				var result =  GetTree().Root.World2D.DirectSpaceState.IntersectRay(quety_parameters);
				if (result.Count == 0)
				{
					return spawnPosition ;
				}
				else{
					
					randomDirection = randomDirection.Rotated(Godot.Mathf.DegToRad(90));
				}	
			}
			return Vector2.Zero;
		}
		private void SpawnEnemy()
		{
			if(_deactivate){return;}	
			player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody2D;
			if(player == null)
			{
				return;
			}
			var enemyScene = enemyTable.PickItem();
			var  enemy = enemyScene.Instantiate() as CharacterBody2D;
			_gameEvents.Connect(game_events.SignalName.WaveFinished , Callable.From(()=>
			{
				(enemy as IEnemy)?.OnWaveFinished();
			}
			));
			GetTree().GetFirstNodeInGroup("EntitiesLayer").AddChild(enemy);
			enemy.GlobalPosition = GetSpawnPosition();
		}
	}
}

