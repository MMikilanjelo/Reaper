using Godot;
using System;

namespace Game.Components
{
public partial class HealthComponent : Node2D
{
	
	[Signal] public delegate void HealthChangedEventHandler(HealthUpdate healthUpdate);
	[Signal] public delegate void DiedEventHandler();
	private bool _hasDied;
	public bool _isDamaged => CurrentHealth < MaxHealth;
	public bool _HasHealthRamaining => !Mathf.IsEqualApprox(CurrentHealth , 0f);

	private float _currentHealth = 15f;
	private float _maxHealth = 15f;
	public bool canAcceptDamage{get ; set;} = true;

	

	[Export]
	public float MaxHealth
	{
		get => _maxHealth;
		private set
		{
			_maxHealth = value;
			if(CurrentHealth > _maxHealth)
			{
				_currentHealth = _maxHealth;
			}
		}
	}
	public float CurrentHealth
	{
		get => _currentHealth;
		private set
		{
			_currentHealth = Mathf.Clamp(value , 0 , MaxHealth);
			var  healthUpdate = new HealthUpdate
			{
				CurrentHealth = _currentHealth,
				MaxHealth  = _maxHealth,
			};
			EmitSignal(SignalName.HealthChanged , healthUpdate);
			if(!_HasHealthRamaining && !_hasDied)
			{
				_hasDied = true;
				EmitSignal(SignalName.Died);
				

			} 
		}

	}
	
	
	public override void _Ready()
	{
		InitializeHealth();
	}

	
	public void SetMaxHealth(float health)
	{
		MaxHealth = health;
	}
	public void InitializeHealth()
	{
		CurrentHealth = MaxHealth;
	}
	public void Damage(float damage )
	{
		if(canAcceptDamage)
		{
			CurrentHealth -= damage;
		}
		
	}
	public partial class HealthUpdate : RefCounted
	{
		public float MaxHealth;
		public float CurrentHealth;
	}
}
}

