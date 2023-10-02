using Godot;
using System;

namespace Enemy.Parts
{
    public partial class VoidHandPart : HandPart
    {
        public VoidHandPart(float Dmg, PartsCounter handsPartCounter , string PathToArt) : base(Dmg, handsPartCounter)
        {
			this.Dmg = Dmg;
			HandTexture = ResourceLoader.Load<Texture2D>(handsPartCounter.pathToArt + PathToArt);
			handsPartCounter.HandsPart.Add(this);
        }

    }
}
