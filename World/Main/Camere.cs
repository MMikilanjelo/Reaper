using Godot;
using System;

public partial class Camere : Camera2D
{
	Vector2 targetPosition = Vector2.Zero;
	Node2D playerNode;
	public override void _Ready()
	{
		MakeCurrent();
		playerNode = GetTree().GetFirstNodeInGroup("Player") as Node2D;
	}

	public override void _Process(double delta)
	{
		_GetTargetPosition();
		GlobalPosition = GlobalPosition.Lerp(targetPosition , (float)(1.0 - Mathf.Exp(-delta * 10)));
	}
	public  void _GetTargetPosition()
	{
		
		if(GetTree().GetFirstNodeInGroup("Player") != null)
		{
			targetPosition = playerNode?.GlobalPosition ?? GlobalPosition;
		}
		else
		{
			return;
		}
		
		
		
	}
}
