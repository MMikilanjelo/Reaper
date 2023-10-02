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
	public static class MathUtil
	{
		public static RandomNumberGenerator RNG { get; private set; } = new();

		static MathUtil()
		{
			RNG.Randomize();
		}

		[Obsolete("Use alternate form of DeltaLerp")]
		public static float DeltaLerp(float smoothing, float delta) => 
			1f - Mathf.Pow(smoothing, delta);

		public static float DeltaLerp(float from, float to, float deltaTime, float smoothing) => 
			Mathf.Lerp(from, to, 1f - Mathf.Exp(-deltaTime * smoothing));

		public static void SeedRandomNumberGenerator(ulong seed) =>
			RNG = new RandomNumberGenerator
			{
				Seed = seed
			};
	}
}

