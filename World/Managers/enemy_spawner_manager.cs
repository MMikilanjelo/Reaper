using Godot;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Managers
{
public partial class enemy_spawner_manager : Node
{
	[Export] PackedScene BasickEnemyScene;
	[Export] Timer EnemySpawnerInterval;
	[Export] const float SPAWN_RADIUS = 150f;
	CharacterBody2D player;
	public static  RandomNumberGenerator random = new RandomNumberGenerator();
	public override void _Ready()
	{
		
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
			
			spawnPosition = player.GlobalPosition +(randomDirection * SPAWN_RADIUS );
			var addditional_check_offset = randomDirection * 30;
			var quety_parameters = PhysicsRayQueryParameters2D.Create(player.Position , spawnPosition  + addditional_check_offset , 1);
			var result =  GetTree().Root.World2D.DirectSpaceState.IntersectRay(quety_parameters);
			
			if (result.Count == 0)
			{
				return spawnPosition ;
			}
			else{
				
				randomDirection = randomDirection.Rotated(Godot.Mathf.DegToRad(90));
			}
		}

		
		
		return spawnPosition;
	}
	private void SpawnEnemy()
	{
		
		player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody2D;
		if(player == null)
		{
			return;
		}

		var  enemy = BasickEnemyScene.Instantiate() as CharacterBody2D;
		GetTree().GetFirstNodeInGroup("EntitiesLayer").AddChild(enemy);
		enemy.GlobalPosition = GetSpawnPosition();
		

	}


}
}

