using System;
using Emotions.Controllers;
using GameManagement;
using UI;
using UnityEngine;

namespace BaseHealth
{
    public abstract class Health : MonoBehaviour
    {
        # region Fields
    
        // health fields
        
        [SerializeField] private float maxHealth;
        
        private float _health;

        private HealthBar _healthBar;


        // iframe variables
        
        public float timeInvincible = 1f;
        
        private float _invincibleTimer;

        # endregion

        # region Methods

        void Start()
        {
            _health = maxHealth;
            _healthBar = HealthBar.Instance;
        }

        public void Heal(float amount)
        {
            _health = Mathf.Clamp(_health + amount, 0, maxHealth);
            _healthBar.SetValue(_health / maxHealth);
        }
        
        public void Damage(float amount)
        {
            if (_invincibleTimer >= 0) return;          // cannot damage while decrementing invincible time
            Reduce(amount);
            _invincibleTimer = timeInvincible;
        }

        public void Reduce(float amount)
        {
            _health -= amount;
            _healthBar.SetValue(_health / maxHealth);


            if (_health < 0) SceneLoader.instance.LoadScene("EntryMenu");
        }

        protected virtual void FixedUpdate()
        {
            if (_invincibleTimer >= 0) _invincibleTimer -= Time.fixedDeltaTime;
        }

        # endregion
    }
}
