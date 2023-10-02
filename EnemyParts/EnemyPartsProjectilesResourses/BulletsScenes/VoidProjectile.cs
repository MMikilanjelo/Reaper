using Godot;
using System;
using Game.Components;
public partial class VoidProjectile : CharacterBody2D
{
	public float MoveSpeed{get ; set;}
	[Export] public  HitBoxComponent hitBoxComponent;
	[Export] Timer timer;
	[Export] VelocityComponent velocityComponent;
	public Vector2  direction {get; set;}
	public override void _Ready()
	{
		TopLevel = true;
		LookAt(direction);
		velocityComponent.SetMaxSpeed(MoveSpeed);
		hitBoxComponent.Connect(HitBoxComponent.SignalName.OnImpackt , new Callable(this , nameof(OnImpackt)));
		timer.Connect(Timer.SignalName.Timeout , new Callable(this , nameof(OnTimeTimeOut)));
		hitBoxComponent.Connect(HitBoxComponent.SignalName.OnWallCollide , new Callable(this, nameof(OnWallColide)));

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
	public override void _Process(double delta)
	{

		velocityComponent.AccelerateInDirection(direction);
		velocityComponent.Move(this);
		
	}
}
