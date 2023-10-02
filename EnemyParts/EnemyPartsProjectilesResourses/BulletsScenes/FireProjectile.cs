using Godot;
using System;
using Game.Components;
public partial class FireProjectile : CharacterBody2D
{
	public float MoveSpeed{get ; set;}
	[Export] public  HitBoxComponent hitBoxComponent;
	[Export] Timer timer;
	[Export] VelocityComponent velocityComponent;
	[Export] PathFindingComponent pathFindingComponent;
	CharacterBody2D player;
	public Vector2  direction {get; set;}
	public override void _Ready()
	{
		player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody2D;
		LookAt(direction);
		TopLevel = true;
		pathFindingComponent.SetTargetPosition(player?.GlobalPosition ?? GlobalPosition);
		velocityComponent.SetMaxSpeed(MoveSpeed);
		hitBoxComponent.Connect(HitBoxComponent.SignalName.OnImpackt , new Callable(this , nameof(OnImpackt)));
		timer.Connect(Timer.SignalName.Timeout , new Callable(this , nameof(OnTimeTimeOut)));
		hitBoxComponent.Connect(HitBoxComponent.SignalName.OnWallCollide , new Callable(this, nameof(OnWallColide)));
		pathFindingComponent.intervalTimer.Start(0.3f);
		pathFindingComponent.intervalTimer.Connect(Timer.SignalName.Timeout , new Callable(this , nameof(SetTargetPos)));
	}
	private void OnImpackt()
	{
		QueueFree();
	}
	private void OnTimeTimeOut()
	{
		QueueFree();
	}
	private void OnWallColide()
	{
		QueueFree();
	}
	private void SetTargetPos()
	{
		if(GetTree().GetFirstNodeInGroup("Player") == null)
		{
			return;
		}
		pathFindingComponent.SetTargetPosition(player?.GlobalPosition ?? GlobalPosition);
	}
	public override void _Process(double delta)
	{
		pathFindingComponent.FollowPath();
		velocityComponent.Move(this);
		
	}
}
