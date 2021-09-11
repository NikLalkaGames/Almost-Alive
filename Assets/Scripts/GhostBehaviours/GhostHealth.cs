using Core.Health;
using Emotions.Controllers;
using GameManagement;
using UnityEngine;
using UnityEngine.UI;

namespace GhostBehaviours
{
    public class GhostHealth : BaseHealth
    {
        // Inspector Constants
        
        [SerializeField] private float healthReductionValue;

        [SerializeField] private float healthReductionIncrement;

        [SerializeField] private float fiveOrbsHealValue;
        
        // UI for hero ghost
        
        [SerializeField] private Slider healthBar;
        
        private void Awake()
        {
            GhostEmotionController.OnFiveOrbsCollected += () => Restore(fiveOrbsHealValue);
            GhostEmotionController.OnFiveOrbsCollected += () => IncreaseHealthReduction(healthReductionIncrement);
        }

        protected override void Start()
        {
            base.Start();
            healthBar.maxValue = MaxHealth;
        }

        public override void Restore(float amount)
        {
            base.Restore(amount);
            healthBar.value = HealthValue;
        }

        public override void Reduce(float amount)
        {
            base.Reduce(amount);
            healthBar.value = HealthValue;
            if (HealthValue <= 0) SceneLoader.instance.LoadScene("EntryMenu");
        }
        
        private void IncreaseHealthReduction(float value = 0.001f) => 
            healthReductionValue += value;
        
        protected override void FixedUpdate() 
        {
            base.FixedUpdate();
            Reduce(healthReductionValue);
        }
        
        #region Test
        
        // TODO: Delete on release
        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                TryToDamage(30);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                Restore(30);
            }
        }
        
        #endregion
    }
    
}