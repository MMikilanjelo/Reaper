using Godot;
using System;


namespace Enemy.Parts
{
    public partial class DuckHead : HeadPart
    {
		
        public DuckHead(PartsCounter handsPartCounter , string  PathToArt ) : base(handsPartCounter)
        {
			HeadTexture = ResourceLoader.Load<Texture2D>(handsPartCounter.pathToArt + PathToArt);
        }
        public override void _Ready()
        {
            GD.Print("Duck duck duck");
        }

    }
}

