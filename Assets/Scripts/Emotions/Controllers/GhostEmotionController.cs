using System;
using GhostBehaviours;
using UnityEngine;

namespace Emotions.Controllers
{
    public class GhostEmotionController : EmotionController
    {
        protected override Vector3 DirectionOfDrop => GhostMovement.Instance.LookDirection;

        /// <summary>
        /// Event occurs when player ghost collect five orbs
        /// </summary>
        public static event Action OnFiveOrbsCollected;

        private void FiveOrbs()
        {
            Debug.Log("5 orbs consumption invocation");

            var capacity = _emotions.Count;
            for (var i = 0; i < capacity; i++)
            {
                ReturnEmotion();            // return emotion to pool
            }
            
            OnFiveOrbsCollected?.Invoke();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_emotions.Count > 0)
                {
                    ThrowEmotion();
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_emotions.Count == 5)
                {
                    // TODO: show ui and replace if statements
                    FiveOrbs();
                }
            }
        }
    }
}
