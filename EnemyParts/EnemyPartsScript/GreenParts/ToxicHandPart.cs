using Godot;
using System;

namespace Enemy.Parts
{
    public partial class ToxicHandPart : HandPart
    {
        public ToxicHandPart(float Dmg, PartsCounter handsPartCounter, string PathToArt) : base(Dmg, handsPartCounter)
        {

        
			this.Dmg = Dmg;
			HandTexture = ResourceLoader.Load<Texture2D>(handsPartCounter.pathToArt + PathToArt);
			handsPartCounter.HandsPart.Add(this);
        
        }
    }
}
