using Godot;
using System;


namespace Enemy.Parts
{
    public partial class ToxicTorsoPart : TorsoPart
    {
        public ToxicTorsoPart(float decresmentDmgProcent, PartsCounter partsCounter, string PathToArt) : base(decresmentDmgProcent, partsCounter)
        {
			this.decresmentDmgProcent = decresmentDmgProcent;
			TorsoTexture = ResourceLoader.Load<Texture2D>(partsCounter.pathToArt + PathToArt);
            partsCounter.TorsoPart.Add(this);
        }
    }

}
