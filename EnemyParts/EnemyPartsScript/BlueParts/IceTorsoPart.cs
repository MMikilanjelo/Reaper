using Godot;
using System;


namespace Enemy.Parts
{
    public partial class IceTorsoPart : TorsoPart
    {
        public IceTorsoPart(float decresmentDmgProcent, PartsCounter partsCounter , string PathToArt) : base(decresmentDmgProcent, partsCounter)
        {
			this.decresmentDmgProcent = decresmentDmgProcent;
			TorsoTexture = ResourceLoader.Load<Texture2D>(partsCounter.pathToArt + PathToArt);
            partsCounter.TorsoPart.Add(this);
        }
        public void  Greating ()
        {
            GD.Print("hello");
        }
    }
}

