using BaseHealth;
using Emotions.Controllers;
using UnityEngine;

namespace GhostBehaviours
{
    public class GhostHealth : Health
    {
        // Inspector Constants
        
        [SerializeField] private float healthReductionValue;

        [SerializeField] private float healthReductionIncrement;

        [SerializeField] private float fiveOrbsHealValue;
        
        private void Awake()
        {
            GhostEmotionController.OnFiveOrbsCollected += () => Heal(fiveOrbsHealValue);
            GhostEmotionController.OnFiveOrbsCollected += () => IncreaseHealthReduction(healthReductionIncrement);
        }
        
        // TODO: Delete on release
        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Reduce(5);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                Heal(30);
            }
        }
        
        protected override void FixedUpdate() 
        {
            base.FixedUpdate();
        
            Reduce(healthReductionValue);
        }
        
        /// <summary>
        /// Ghost health reduction incrementing
        /// </summary>
        private void IncreaseHealthReduction(float value = 0.001f) => healthReductionValue += value;
    }
    
}