using Godot;
using System;
using Enemy.Parts;
namespace Game.Components
{
	public partial class SpriteHandlerComponent : Node
	{
		[Export] EnemyConstructor enemyConstructor = null;
		[Export] Sprite2D HandSprite;
		[Export] Sprite2D LegsSprite;
		[Export] Sprite2D TorsoSprite;
		[Export] Sprite2D HeadSprite;
		public override void _Ready()
		{
			//HandSprite.Texture = enemyConstructor.handPart.texture;
			//LegsSprite.Texture = enemyConstructor.legsPart.texture2D;
			//TorsoSprite.Texture = enemyConstructor.torsoPart.texture2D;
			//HeadSprite.Texture = enemyConstructor.headPart.texture;
		}
		
	}
}

