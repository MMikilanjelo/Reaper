using Godot;
using System;

public partial class Cursor : Sprite2D
{
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Hidden;
        GlobalPosition = GetGlobalMousePosition();
    }
    public override void _PhysicsProcess(double delta)
    {
       
       GlobalPosition = GetGlobalMousePosition();
       ForceUpdateTransform();
    }
}
