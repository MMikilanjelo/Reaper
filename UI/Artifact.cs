using Godot;
using System;

namespace GameUI
{
	public partial class Artifact : TextureRect
	{
        public void InitializeArtifact(ShopSlotData _artifactData)
		{
			Texture = _artifactData._itemTexture;
			var _artifactInstance = _artifactData._itemScene.Instantiate() as IVisitor;
			AddChild(_artifactInstance as Node2D);
			var player = GetTree().GetFirstNodeInGroup("Player") as PlayerController;
			player.Accept(_artifactInstance);
		}
	}
}

