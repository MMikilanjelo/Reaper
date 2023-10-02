using Godot;
using System;
using System.Collections.Generic;
using System.Threading;


namespace Game.Components
{
	
	public partial class BulletPatternComponent : Node2D
	{
		[Export] Path2D bulletPatternPath;
		[Export] PackedScene bulletScene;
	
		public override void _Ready()
		{
		
			foreach(var bulletPosition in GetbulletPositions())
			{
				
				Bullet bulletInstance = bulletScene.Instantiate() as Bullet;
				bulletInstance.Position = bulletPosition;
				AddChild(bulletInstance);
			}

			
			
		}
		
		private List<Vector2> GetbulletPositions()
		{
			List<Vector2> bulletPositions  = new();
			for(int i =  0  ; i < bulletPatternPath.Curve.PointCount ; i ++)
			{
				bulletPositions.Add(bulletPatternPath.Curve.GetPointPosition(i));
			}
			return bulletPositions;
		}
	}
}

