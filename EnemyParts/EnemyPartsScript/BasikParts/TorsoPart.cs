using Godot;
using System;

namespace Enemy.Parts
{
	public partial class TorsoPart : Node2D , ITorso
	{
		public float decresmentDmgProcent{get; protected set;}
		protected Texture2D TorsoTexture;
        public Texture2D texture2D => TorsoTexture;

        public void DecreseDmgByProcent(float DecreseDmgByProcent)
		{

		}
		public override void _Ready()
		{

			
		}
		public void AttachPart(CharacterBody2D parentNode )
		{
			CallDeferred(nameof(AddPart) ,parentNode);
		}
		private void AddPart(CharacterBody2D ParentNode)
		{
			ParentNode.AddChild(this);
		}
		public TorsoPart( float decresmentDmgProcent , PartsCounter partsCounter )
		{

			
		}

	}
public interface ITorso
{
	Texture2D texture2D{get;}
	void DecreseDmgByProcent(float percent);
	void AttachPart(CharacterBody2D parentNode);
	
}
}

