using Godot;
using System;


namespace Game.Components
{
	public partial class DeathSceneComponent : Node2D
	{
		CharacterBody2D player;
		[Export] PackedScene BloodParticle;
		[Export] HealthComponent healthComponent;

        public override void _Ready()
        {
			healthComponent.Connect(HealthComponent.SignalName.Died , Callable.From(()=>
			{
				OnEnemyDied();
			}));
			
			player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody2D;
            
        }
        public void SpawnBloodPartickle(Vector2 Position)
		{
			  
        	var blood = BloodParticle.Instantiate() as GpuParticles2D;
       		GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(blood);
        	blood.GlobalPosition = Position;
        	blood.Rotation  = GlobalPosition.AngleToPoint(player.GlobalPosition);
		}
		public void OnEnemyDied()
		{
			if(this.Owner == null || Owner is not Node2D)
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
		}
	}
}


