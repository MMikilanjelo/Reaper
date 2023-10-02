using Godot;
using System;


namespace Enemy.Parts
{
    public partial class CrabsHand : HandPart
    {
        public CrabsHand(float Dmg, PartsCounter handsPartCounter , string PathToArt) : base(Dmg, handsPartCounter)
        {
			this.Dmg = Dmg;
			HandTexture = ResourceLoader.Load<Texture2D>(handsPartCounter.pathToArt  + PathToArt);
			handsPartCounter.HandsPart.Add(this);
        }
    }
}

