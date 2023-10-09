using Godot;
using System;

public partial class StatUI : CanvasLayer
{
    public override void _Ready()
    {
		
        GetTree().Paused = true;
		
	}
    public override void _Process(double delta)
    {
		if(Input.IsActionJustPressed("CloseTab"))
		{
			CloseTab();
		}
    }
    public  void CloseTab()
	{
		GetTree().Paused = false;
		QueueFree();
	}

}
