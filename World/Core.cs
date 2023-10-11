using Godot;
using System.Collections.Generic;
using Generation;
using Enemy.Parts;
using System.Security.Cryptography.X509Certificates;
using Game.Components;
using Generation.Alghoritms;
public partial class Core : Node2D
{
	UIEvents Uievents;
	[Export] private WorldGeneratorComponent worldGenerator;
	[Export] TileMap tileMap;
	[Export] PackedScene StatUIScene;
	public override void _Ready()
	{
		
	}
	

	public override void _Process(double delta)
	{
		GD.Print(Engine.GetFramesPerSecond());
		if(Input.IsActionJustPressed("OpenTab"))
		{

			var statMenu = StatUIScene.Instantiate() as StatUI;
			AddChild(statMenu);
		}
		
	}
}
