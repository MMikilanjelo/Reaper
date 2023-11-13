using Game.Components;
using Godot;
using System;

public partial class experience : Node2D
{
	[Export] Area2D PickableArea;
	 game_events game_Events;
	public override void _Ready()
	{
		game_Events = GetNode<game_events>("/root/GameEvents");
		PickableArea.Connect(Area2D.SignalName.AreaEntered , new Callable(this , nameof(OnAreaEntered)));
		
	}

	private void  OnAreaEntered(Area2D oterArea)
	{
		if(oterArea is HitBoxComponent)
		{
			return;
		}
		game_Events.On_ExperienceVialCollected(1);
		QueueFree();
	}
	
}
