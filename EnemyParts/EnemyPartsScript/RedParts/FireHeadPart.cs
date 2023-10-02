using Godot;
using System;


namespace Enemy.Parts
{
    public partial class FireHeadPart : HeadPart
    {
        public FireHeadPart(PartsCounter partsCounter , string PathToArt , float AtackTime ) : base(partsCounter)
        {
            this.AtackTime = AtackTime;
			HeadTexture = ResourceLoader.Load<Texture2D>(partsCounter.pathToArt + PathToArt);
            partsCounter.HeadPart.Add(this);
            HeadRes = ResourceLoader.Load<HeadRes>("res://EnemyParts/EnemyPartsProjectilesResourses/Resourses/FireHeadResourse.tres");
        }
       public override void  DoSomethisngSpecial( CharacterBody2D Parent , CharacterBody2D player )
		{
			FireProjectile bulletInstance = HeadRes.projectileScene.Instantiate() as FireProjectile;
			bulletInstance.MoveSpeed = HeadRes.projectileMoveSpeed;
			bulletInstance.Position = this.GetParent<CharacterBody2D>().Position;
			bulletInstance.direction  = (player.GlobalPosition - GlobalPosition).Normalized();
			AddChild(bulletInstance);
		}
        
    }
}

