using Core.Health.Interfaces;
using UnityEngine;

namespace Core.Health
{
    public abstract class BaseHealth : MonoBehaviour, IHealable, IDamageable
    {
        # region Fields
        
        #region Health
        
        [SerializeField] protected float maxHealth;     // 100
        
        protected float _healthValue;
        
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

        public float Value => _healthValue;
        
        #endregion

        # region Methods
        
        protected virtual void Start() => _healthValue = maxHealth;

        public virtual void Restore(float amount) =>
            _healthValue = Mathf.Clamp(_healthValue + amount, 0, maxHealth);
        
        public virtual void TryToDamage(float amount)
        {
            if (_invincibleTimer >= 0) return;
            var damage = Mathf.Clamp(amount - defense, 0f, Mathf.Infinity );
            Reduce(damage);
            _invincibleTimer = timeInvincible;
        }

        public virtual void Reduce(float amount) => 
            _healthValue -= amount;

        protected virtual void FixedUpdate()
        {
            if (_invincibleTimer >= 0) _invincibleTimer -= Time.fixedDeltaTime;
        }

        # endregion
    }
}
