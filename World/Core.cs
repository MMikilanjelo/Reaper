using Godot;
using System.Collections.Generic;
using Generation;
using Enemy.Parts;
using System.Security.Cryptography.X509Certificates;
using Game.Components;
using Generation.Alghoritms;
public partial class Core : Node2D
{
	[Export] private WorldGeneratorComponent worldGenerator;
	[Export] TileMap tileMap;
	public override void _Ready()
	{
		
		 
	}
	

	public override void _Process(double delta)
	{
		
	}
}
