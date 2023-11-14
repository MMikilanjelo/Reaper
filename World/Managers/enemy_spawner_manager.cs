using Godot;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using GameLogick;
namespace Managers
{
	public partial class enemy_spawner_manager : Node
	{
		[Export] PackedScene BasickEnemyScene;
		[Export] PackedScene BatEnemyScene;
		[Export] PackedScene KnigthEnemyScene;
		
		[Export] Timer EnemySpawnerInterval;
		[Export] const float SPAWN_RADIUS = 100f;
		[Export] private bool Activate = false;
		CharacterBody2D player;
		public static  RandomNumberGenerator random = new RandomNumberGenerator();
		private readonly LootTable<PackedScene> enemyTable = new LootTable<PackedScene>();
		public override void _Ready()
		{
			enemyTable.AddItemToTable(KnigthEnemyScene , 30);
			enemyTable.AddItemToTable(BasickEnemyScene , 20);
			enemyTable.AddItemToTable(BatEnemyScene , 10);

			EnemySpawnerInterval.Connect(Timer.SignalName.Timeout , new Callable(this, nameof(SpawnEnemy)));
			
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
			if(!Activate){return;}	
			player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody2D;
			if(player == null)
			{
				return;
			}
			var enemyScene = enemyTable.PickItem();
			var  enemy = enemyScene.Instantiate() as CharacterBody2D;
			GetTree().GetFirstNodeInGroup("EntitiesLayer").AddChild(enemy);
			enemy.GlobalPosition = GetSpawnPosition();
			

		}


	}
}

