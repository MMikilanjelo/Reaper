using Godot;
using System;

namespace Enemy.Parts
{
	public partial class HeadPart : Node2D , IHead
	{
		protected Texture2D HeadTexture;
		protected Texture2D BulletTexture;
		protected float AtackTime;
		protected HeadRes HeadRes;
		public float timeToAtack {get => AtackTime;}
		
        public Texture2D texture { get => HeadTexture;}
		public Texture2D projectileTexture{ get => BulletTexture;}
		
		public void AttachPart(CharacterBody2D ParentNode)
		{
			CallDeferred(nameof(AddPart) , ParentNode);
		}
		private void AddPart(CharacterBody2D ParentNode)
		{
			ParentNode.AddChild(this);
		}
		
		public HeadPart (PartsCounter partsCounter)
		{
			
			
			
		} 
		public virtual void DoSomethisngSpecial( CharacterBody2D Parent , CharacterBody2D player )
		{
		
		}
	}
	public interface IHead
	{
		Texture2D texture{get;}
		Texture2D projectileTexture {get;}
		void AttachPart(CharacterBody2D parentNode);
		void DoSomethisngSpecial( CharacterBody2D Parent , CharacterBody2D player);
		float timeToAtack {get;}
		

	}

}
