using Godot;
using Game.Components;
using System.Linq;
using System.Collections;
using System;

namespace Enemy.Parts
{
	
	public partial class EnemyConstructor : Node
	{
		RandomNumberGenerator random;
		public 	IHand handPart{get;set;}
		public 	ITorso torsoPart{get;set;}
		public 	ILegs legsPart{get;set;}
		public IHead headPart{get;set;}

	
		public override void _Ready()
		{
			CharacterBody2D Parent = GetParent<CharacterBody2D>();
			random = new RandomNumberGenerator();
			random.Randomize();
			PartsCounter partsManager = new PartsCounter();
			
			
			IHand ToxicHand = new ToxicHandPart(1 , partsManager , "Toxic_Hands.png");
			IHand VoidHand = new VoidHandPart(1 , partsManager , "Void_Hands.png");
			IHand IceHand = new IceHandPart(1 , partsManager , "Frost_Hands.png");
			IHand FireHand = new FireHandPart(1 , partsManager , "FIre_Hands.png");



			
			
			IHead toxicHead = new ToxicHeadPart(partsManager , "Toxic_Head.png" , 2 );
			IHead fireHead = new FireHeadPart(partsManager  , "Fire_Head.png" , 3  );
			IHead voidHead = new VoidHeadPart(partsManager , "Void_Head.png" , 4 );
			IHead iceHead = new IceHeadPart(partsManager , "Frost_Head.png" , 5 );



			ILegs toxicLegs = new ToxicLegsPart(1,partsManager , "Toxic_Legs.png" ,5);
			ILegs fireLegs = new FireLegsPart(1,partsManager , "Fire_LEgs.png" , 5);
			ILegs voidLegs= new VoidLegsPart(1,partsManager , "Void_Legs.png" , 5);
			ILegs frostzegs = new IceLegsPart(1 , partsManager , "Frost_Legs.png" , 5);	

			ITorso toxicTorso = new ToxicTorsoPart(1 , partsManager , "Toxic_Torso.png");
			ITorso fireTorso = new FireTorsoPart(1 , partsManager , "Fire_Torso.png");
			ITorso voidTorso = new VoidTorsoPart(1 , partsManager , "Void_Torso.png");
			ITorso iceTorso = new IceTorsoPart(1 , partsManager , "Frost_Torso.png");
			ConstructRandomEnemyByParts(partsManager);
			AttachParts(Parent);
			
		}

		private void GetRandomHandPart(Godot.Collections.Array<Node> HandsPart)
		{
			HandsPart.OrderBy(part => Guid.NewGuid());	
			this.handPart = (IHand)(HandsPart[random.RandiRange(0 , HandsPart.Count - 1)]);
		}
		private void  GetRandomTorsoPart(Godot.Collections.Array<Node> TorsoParts)
		{
			TorsoParts.OrderBy(part => Guid.NewGuid());
			this.torsoPart = (ITorso)(TorsoParts[random.RandiRange(0 , TorsoParts.Count - 1)]);
		}
		private void GetRandomLegsPart(Godot.Collections.Array<Node> LegsPart)
		{
			LegsPart.OrderBy(part => Guid.NewGuid());
			this.legsPart = (ILegs)(LegsPart[random.RandiRange(0 , LegsPart.Count - 1)]);
		}
	    private void GetRandomHeadPart(Godot.Collections.Array<Node> HeadPart)
		{

			HeadPart.OrderBy(part => Guid.NewGuid());
			this.headPart = (IHead)(HeadPart[random.RandiRange(0 , HeadPart.Count - 1)]);
		}
		private void ConstructRandomEnemyByParts(PartsCounter partsCounter)
		{
			GetRandomHandPart(partsCounter.HandsPart);
			GetRandomLegsPart(partsCounter.LegsPart);
			GetRandomTorsoPart(partsCounter.TorsoPart);
			GetRandomHeadPart(partsCounter.HeadPart);
		}
		private void AttachParts( CharacterBody2D Parent)
		{
			handPart.AttachPart(Parent);
			legsPart.AttachPart(Parent);
			torsoPart.AttachPart(Parent);
			headPart.AttachPart(Parent);

		}

	
	}
	public partial class PartsCounter : RefCounted
	{
		public readonly string pathToArt = "res://Art Assets/Entity/EntityParts/";
		public Godot.Collections.Array<Node> HandsPart = new Godot.Collections.Array<Node>();
		public  Godot.Collections.Array<Node> TorsoPart =  new Godot.Collections.Array<Node>();
		public  Godot.Collections.Array<Node>  LegsPart = new Godot.Collections.Array<Node>();
		public  Godot.Collections.Array<Node>  HeadPart = new Godot.Collections.Array<Node>();

		
	}
}

