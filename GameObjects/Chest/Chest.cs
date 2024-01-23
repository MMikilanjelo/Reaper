using Game.Components;
using Godot;
using System;

public partial class Chest : Node2D
{
	private game_events Game_Events;
	Area2D interactableArea;
	AnimationPlayer animationPlayer;
	[Export]  public int Chest_bullet_drop = 5;
	private bool is_Collected = false;
	public override void _Ready()
	{
		interactableArea = GetNode<Area2D>("Area2D");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		Game_Events = GetNode<game_events>("/root/GameEvents");
		interactableArea.Connect(Area2D.SignalName.BodyEntered , Callable.From((Node2D otherBody)=>
		{
			if(otherBody is PlayerController)
			{
        		OnInterackt(otherBody);
			}
			
		}));
		GetTree().CreateTimer(6).Connect(Timer.SignalName.Timeout , Callable.From(()=>
		{
			QueueFree();
		}));
	}

	
	public void OnInterackt(Node2D otherBody)
	{
		if(is_Collected){
			return;
		}
		is_Collected = true;
		Game_Events.EmitChestCollection(Chest_bullet_drop);
		animationPlayer.Play("collected");
		animationPlayer.Connect(AnimationPlayer.SignalName.AnimationFinished , Callable.From((string animationName)=>
		{
			if(animationName == "collected")
			{
				QueueFree();
			}
			
		}));
		
	}
}
