using Godot;
using System.Collections.Generic;
using System.Linq;
namespace Game.Components

{
	public partial class VelocityComponent : Node , IVisitable
	{
		[Export] 
		private float maxSpeed = 100;
		[Export]
		private float accelerationCoeficient = 10;
		
		public Vector2 Velocity {get;set;}
		public float SpeedMultiplier{get;set;}  = 1f;
		public float SpeedPercentModifire => speedPercentModifires.Values.Sum();
		public float AccelerationCoeficient => accelerationCoeficient;
		public float AccelerationCoefficientMultiplier{get; set;} = 1f;

		
		public float SpeedPercent => Mathf.Min(Velocity.Length() / (CalculatedMaxSpeed > 0f ? CalculatedMaxSpeed : 1f),1f);
		private float CalculatedMaxSpeed => maxSpeed * SpeedMultiplier * (1f + SpeedPercentModifire);

		private Dictionary<string , float> speedPercentModifires = new Dictionary<string, float>();

		public void AccelerateToVelocity(Vector2 velocity)
		{
			Velocity = Velocity.Lerp(velocity  , 1f - Mathf.Exp(-accelerationCoeficient * AccelerationCoefficientMultiplier * (float)GetProcessDeltaTime()));
		}
		public void AccelerateInDirection(Vector2 direction)
		{
			AccelerateToVelocity(direction * CalculatedMaxSpeed);
		}

		public Vector2 GetMaxVelocity(Vector2 direction)
		{
			return direction * CalculatedMaxSpeed;
		}
		public void MaximizeVelocity(Vector2 direction)
		{
			Velocity = GetMaxVelocity(direction);
		}
		public void Decelerate()
		{
			AccelerateToVelocity(Vector2.Zero);
		}
		public void Move(CharacterBody2D characterBody)
		{
			characterBody.Velocity =  Velocity;
			characterBody.MoveAndSlide();
		}
		public void SetMaxSpeed(float newSpeed)
		{
			maxSpeed = newSpeed;
		}
		public void AddSpeedPercentModifire(string _name , float _value)
		{
			if (!speedPercentModifires.ContainsKey(_name))
        	{
            	speedPercentModifires.Add(_name, _value);
        	}
		}
		public void SetSpeedPercentModifire(string _name  , float _amount)
		{
			if (speedPercentModifires.ContainsKey(_name))
			{
				speedPercentModifires[_name] = _amount;
			}
		}
		public void RemoveSpeedPercentModifire(string _name)
		{
			if(speedPercentModifires.ContainsKey(_name))
			{
				speedPercentModifires.Remove(_name);
			}
		}
		public float? GetSpeedPercentModifier(string name)
		{
			if (speedPercentModifires.ContainsKey(name))
			{
				return speedPercentModifires[name];
			}
			else
			{
				return null;
			}
		}

        public void Accept(IVisitor visitor)
        {
			visitor.Visit(this);
        }
    }
}

