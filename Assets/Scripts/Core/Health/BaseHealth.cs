using Core.Health.Interfaces;
using UnityEngine;

namespace Core.Health
{
    public abstract class BaseHealth : MonoBehaviour, IHealable, IDamageable
    {
        # region Fields
        
        #region Health
        
        [SerializeField] private float maxHealth;     // 100

        private float _healthValue;
        
        #endregion 
        
        #region Statuses
        
        [Tooltip("Reduces the total damage taken by this number (min of 1)")]
        public float defense = 0;
        
        [Tooltip("Doesn't die when reaching 0 health")]
        public bool immortal = false;

        #endregion
        
        #region Invincible timer
        
        public float timeInvincible = 1f;
        
        private float _invincibleTimer;

        #endregion
        
        # endregion
        
        #region Properties

        protected float HealthValue
        {
            get => _healthValue;
            set => _healthValue = Mathf.Clamp(_healthValue + value, 0, MaxHealth);
        }

        protected float MaxHealth => maxHealth;
        
        #endregion

        # region Methods
        
        protected virtual void Start() => HealthValue = MaxHealth;

        public virtual void Restore(float amount) =>
            HealthValue += amount;
        
        public virtual void TryToDamage(float amount)
        {
            if (_invincibleTimer >= 0) return;

            var damage = Mathf.Clamp(amount - defense, 0f, Mathf.Infinity );
            Reduce(damage);
            
            _invincibleTimer = timeInvincible;
        }

        public virtual void Reduce(float amount) => 
            HealthValue -= amount;

        protected virtual void FixedUpdate()
        {
            if (_invincibleTimer >= 0) _invincibleTimer -= Time.fixedDeltaTime;
        }

        # endregion
    }
}
