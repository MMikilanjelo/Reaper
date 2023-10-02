using Godot;
using System;




namespace Enemy.Parts
{
    public partial class VoidLegsPart : LegsPart
    {
        public VoidLegsPart(float moveSpeedIncrisment, PartsCounter partsCounter,string PathToArt , float NavigationInterval) : base( partsCounter)
        {
			this.moveSpeedIncrisment = moveSpeedIncrisment;
            this.NavigationInterval = NavigationInterval;
			LegsTexture = ResourceLoader.Load<Texture2D>(partsCounter.pathToArt + PathToArt);

        }
    }
}

