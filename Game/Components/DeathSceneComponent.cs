using Godot;
using System;
using GameLogick.Utilities;
using Game.Enteties;

namespace Game.Components
{
	public partial class DeathSceneComponent : Node2D
	{
		CharacterBody2D player;
		[Export] PackedScene BloodParticle;
		[Export] HealthComponent healthComponent;
		[Export] RandomAudioPlayer randomAudioPlayer;
		
		game_events _gameEvents;

        public override void _Ready()
        {
			
			_gameEvents = GetNode<game_events>("/root/GameEvents");
			player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody2D;
            healthComponent.Connect(HealthComponent.SignalName.Died , Callable.From(()=>
			{
				OnEnemyDied();
			}));
			
        }
        public void SpawnBloodPartickle(Vector2 Position)
		{
			if(!GameUtilities.CheckIfPlayerExist(this)) return;
        	var blood = BloodParticle.Instantiate() as GpuParticles2D;
       		GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(blood);
        	blood.GlobalPosition = Position;
        	blood.Rotation  = GlobalPosition.AngleToPoint(player?.GlobalPosition ?? GlobalPosition);
		}
		public void OnEnemyDied()
		{
			if(Owner == null || Owner is not Node2D)
			{
				return;
			}
			var owner = Owner as CharacterBody2D;
			var spawnPosition = owner.GlobalPosition;
			var enteties = GetTree().GetFirstNodeInGroup("EntitiesLayer");
			GetParent().RemoveChild(this);
			enteties.AddChild(this);
			GlobalPosition = spawnPosition;
			SpawnBloodPartickle(spawnPosition);
			randomAudioPlayer.PlayRandom();
			randomAudioPlayer.Connect(AudioStreamPlayer.SignalName.Finished , Callable.From(()=>
				QueueFree()
			));
		}
	}
}


