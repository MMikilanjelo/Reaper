using Godot;
using System;
using System.Runtime.CompilerServices;
using GameUI;
using Game.Enteties;
namespace Game.Components
{
    public partial class HealthComponent : Node2D , IVisitable
    {
        

        [Signal] public delegate void HealthChangedEventHandler(HealthUpdate healthUpdate);
        [Signal] public delegate void DiedEventHandler();
        private bool _hasDied;
        private PackedScene floatingTextScene;
        public bool _isDamaged => CurrentHealth < MaxHealth;
        public bool _HasHealthRamaining => !Mathf.IsEqualApprox(CurrentHealth, 0f);

        private float _currentHealth = 15f;
        private float _maxHealth = 15f;

        public bool canAcceptDamage { get; set; } = true;



        [Export]
        public float MaxHealth
        {
            get => _maxHealth;
            private set
            {
                _maxHealth = value;
                if (CurrentHealth > _maxHealth)
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
                var _previousHealth = _currentHealth;
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                var healthUpdate = new HealthUpdate
                {
                    CurrentHealth = _currentHealth,
                    MaxHealth = _maxHealth,
                    PreviousHEalth = _previousHealth,
                    isHeal = _previousHealth < _currentHealth,
                };

                EmitSignal(SignalName.HealthChanged, healthUpdate);
                if (!_HasHealthRamaining && !_hasDied)
                {
                    _hasDied = true;
                    EmitSignal(SignalName.Died);
                }
            }
        }



        public override void _Ready()
        {
            InitializeHealth();
            floatingTextScene = ResourceLoader.Load("res://UI/FloatingText.tscn") as PackedScene;
        }


        public void SetMaxHealth(float health)
        {
            MaxHealth = health;
        }
        public void InitializeHealth()
        {
            CurrentHealth = MaxHealth;
        }
        public void Damage(float damage)
        {
            if (canAcceptDamage)
            {
                CurrentHealth -= damage;
                var floating_text = floatingTextScene.Instantiate() as FloatingText;
                GetTree().GetFirstNodeInGroup("ForeGroundLayer").AddChild(floating_text);
                floating_text.GlobalPosition = GlobalPosition;
                floating_text.Start(Convert.ToString(damage));
            }

        }
        public void SetCurrentHealth(int amount)
        {
            CurrentHealth += amount;
        }
        public void IncreaseMaxHealth(int amount)
        {
            MaxHealth += amount;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
            GD.Print(visitor + " Visited");
        }

        public partial class HealthUpdate : RefCounted
        {
            public float MaxHealth;
            public float CurrentHealth;
            public float PreviousHEalth;
            public bool isHeal;
        }
    }
}

