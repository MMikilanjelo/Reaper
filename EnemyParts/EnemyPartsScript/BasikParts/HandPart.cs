using Godot;
using System;
using Game.Components;


namespace Enemy.Parts
{
	public partial class HandPart : Node2D , IHand 
	{
		protected Texture2D HandTexture;
		public float Dmg{get;protected set;}
        public Texture2D texture { get => HandTexture;}

        public override void _Ready()
		{
			
			
		}
		public void AttachPart(CharacterBody2D ParentNode)
		{
			CallDeferred(nameof(AddPart) , ParentNode);
		}
		private void AddPart(CharacterBody2D ParentNode)
		{
			ParentNode.AddChild(this);
		}
		
		public HandPart (float Dmg ,PartsCounter handsPartCounter)
		{
		}
		
		
	}
	public interface IHand
	{
		Texture2D texture{get;}
		void AttachPart(CharacterBody2D parentNode);
		
	}
	


}


