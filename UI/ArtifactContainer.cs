using Godot;
using System;


namespace GameUI
{
	public partial class ArtifactContainer : GridContainer 
	{
		PackedScene _artifactScene;
		game_events _gameEvents;
		public override void _Ready()
		{
			_gameEvents = GetNode<game_events>("/root/GameEvents");
			_artifactScene = ResourceLoader.Load<PackedScene>("res://UI/Artifact.tscn");
			_gameEvents.Connect(game_events.SignalName.ShopSlotPurchased , Callable.From((ShopSlotData _slotData) =>
			{
				if(_slotData._itemType == ShopSlotData.ItemType.Artifact)
				{
					SetArtifact(_slotData);
				}
			}));
		}
		public void SetArtifact(ShopSlotData _artifactData)
		{
			var _artifactInstance = _artifactScene.Instantiate() as Artifact;
			AddChild(_artifactInstance);
			_artifactInstance.InitializeArtifact(_artifactData);
		}
	}
}

