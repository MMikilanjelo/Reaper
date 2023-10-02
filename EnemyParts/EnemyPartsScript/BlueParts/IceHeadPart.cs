using Godot;
using System;


namespace Enemy.Parts
{

    public partial class IceHeadPart : HeadPart
    {
        public IceHeadPart(PartsCounter partsCounter,   string PathToArt , float AtackTime) : base(partsCounter)
        {
            this.AtackTime = AtackTime;
            HeadTexture = ResourceLoader.Load<Texture2D>(partsCounter.pathToArt + PathToArt);
            
            HeadRes = ResourceLoader.Load<HeadRes>("res://EnemyParts/EnemyPartsProjectilesResourses/Resourses/IceHadResourse.tres");
            partsCounter.HeadPart.Add(this);
        
        }

        public override void  DoSomethisngSpecial( CharacterBody2D Parent , CharacterBody2D player )
		{
			IceArrow bulletInstance = HeadRes.projectileScene.Instantiate() as IceArrow;
			bulletInstance.MoveSpeed = HeadRes.projectileMoveSpeed;
			bulletInstance.Position = this.GetParent<CharacterBody2D>().Position;
			bulletInstance.direction  = (player.GlobalPosition - GlobalPosition).Normalized();
			AddChild(bulletInstance);
		}
    }
}

