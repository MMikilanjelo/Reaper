using Godot;
using System;

namespace GameLogick.Utilities
{
	public   static class GameUtilities 
	{
		public static bool CheckIfPlayerExist(Node2D node)
		{
			if(node.GetTree().GetFirstNodeInGroup("Player") == null)
			{
				return false;
			}
			else{
				return true;
			}
		}
		public static CharacterBody2D GetPlayerNode(Node2D node)
		{
			return node.GetTree().GetFirstNodeInGroup("Player") as CharacterBody2D;
		}
	}
}

