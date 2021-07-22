using GameManagement;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using static Emotions.Controllers.GhostEmotionController;

namespace GhostBehaviours
{
    public class GhostHealth : MonoBehaviour
    {
        # region Fields
    
        // health fields
        
        [SerializeField] private float maxHealth;
        
        private float _health;
        
        [SerializeField] private float healthReductionValue;
        
        private HealthBar _healthBar;

        
        // iframe variables
        
        public float timeInvincible = 1f;
        
        public float invincibleTimer;

        # endregion

        # region Methods

        private void Awake()
        {
            OnGhostHeal += UpdateHealth;
            OnGhostHeal += IncreaseHealthReduction;
        }

        void Start()
        {
            _health = maxHealth;
            _healthBar = HealthBar.Instance;
        }

        public void Heal(float healAmount)
        {
            _health = Mathf.Clamp(_health + healAmount, 0, maxHealth);
            _healthBar.SetValue(_health / maxHealth);
            Debug.Log("Health has been restored: " + _health / maxHealth);
        }
        
        public void Damage(float damageAmount)
        {
            if (invincibleTimer < 0)
            {
                _health += damageAmount;
                if (_health > 0)
                {
                    _healthBar.SetValue(_health / maxHealth);
                    invincibleTimer = timeInvincible;
                    Debug.Log("Health has been reduced: " + _health / maxHealth);
                }
                else
                {
                    SceneLoader.instance.LoadScene("EntryMenu");            // TODO: later change to restart scene
                }
            }
        }

        public void UpdateHealth(float amount)
        {
            if (amount < 0)
            {
                Damage(amount);
            }
            else
            {
                Heal(amount);
            }
        }


        // TODO: Delete on release
        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                UpdateHealth(-10);
                invincibleTimer -= 0.1f;
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                UpdateHealth(+30);
            }
        }

        private void FixedUpdate() 
        {
            if (invincibleTimer >= 0) invincibleTimer -= Time.fixedDeltaTime;
        
            if (_health > 0)
            {
                _health -= healthReductionValue;
                _healthBar.SetValue(_health / maxHealth);
            }
            else
            {
                SceneLoader.instance.LoadScene("EntryMenu");
            }
        }

        public void IncreaseHealthReduction(float value = 0.001f) => healthReductionValue += value;

        # endregion
    }
}
