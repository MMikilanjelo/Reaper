using Godot;
using System;


namespace Enemy.Parts
{
    public partial class VoidHeadPart : HeadPart
    {
        public VoidHeadPart(PartsCounter headPartsCounter , string PathToArt , float AtackTime ) : base(headPartsCounter)
        {
			this.AtackTime = AtackTime;

			HeadTexture = ResourceLoader.Load<Texture2D>(headPartsCounter.pathToArt + PathToArt);
			HeadRes = ResourceLoader.Load<HeadRes>("res://EnemyParts/EnemyPartsProjectilesResourses/Resourses/VoidHeadResourse.tres");
        }
       public override void  DoSomethisngSpecial(CharacterBody2D Parent , CharacterBody2D player)
		{
			VoidProjectile bulletInstance =  HeadRes.projectileScene.Instantiate() as VoidProjectile;
			bulletInstance.MoveSpeed = HeadRes.projectileMoveSpeed;
			bulletInstance.Position = this.GetParent<CharacterBody2D>().Position;
			bulletInstance.direction  = (player.GlobalPosition - GlobalPosition).Normalized();
			AddChild(bulletInstance);
		}
    }
}

