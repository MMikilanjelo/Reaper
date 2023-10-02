using Godot;
using System;


namespace Enemy.Parts
{
	public  partial class LegsPart : Node2D , ILegs
	{
		
		protected Texture2D LegsTexture;
		public Texture2D texture2D => LegsTexture;
		public float moveSpeedIncrisment{get ; protected set;}
		protected float NavigationInterval;

		public float ChangeNavigationInterval{get => NavigationInterval;}
		public void IncreaseMoveSpeed(float moveSpeedIncrisment)
		{
			
		}
		public override void _Ready()
		{
			
		}
		public void AttachPart(CharacterBody2D parentNode)
		{
			CallDeferred(nameof(AddPart) , parentNode);
		}
		private void AddPart(CharacterBody2D ParentNode)
		{
			ParentNode.AddChild(this);
		}
		public LegsPart( PartsCounter partsCounter)
		{
			
			
		}
	}
	public interface ILegs
	{
		Texture2D texture2D{get;}
		void IncreaseMoveSpeed(float moveSpeedIncrisment);

		void AttachPart(CharacterBody2D parentNode);

		float ChangeNavigationInterval{get;}
	}
}

