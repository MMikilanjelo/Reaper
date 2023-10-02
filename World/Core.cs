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
		
		//worldGenerator.RunProceduralGeneration();
		// tileMap.SetCell(0,new Vector2I(0,-10) ,0,new Vector2I(1 ,1 ),0);
		// foreach(var direction in Directions.allDirectionsList)
		// {
		// 	tileMap.SetCell(0,new Vector2I(0,-10) + direction ,0,new Vector2I(3,3 ),0);
		// }
		 
	}
	

	public override void _Process(double delta)
	{
		
	}
}
