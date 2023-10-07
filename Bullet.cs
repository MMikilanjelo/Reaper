using Godot;
using System;
using Game.Components;
public partial class Bullet : CharacterBody2D 
{

	public float MoveSpeed{get ; set;}
	[Export] protected  HitBoxComponent hitBoxComponent;
	[Export] protected Timer timer;
	[Export] protected VelocityComponent velocityComponent;
	public Vector2  direction {get; set;}
	private int a ;


	public override void _Ready()
	{
		
		TopLevel = true;
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