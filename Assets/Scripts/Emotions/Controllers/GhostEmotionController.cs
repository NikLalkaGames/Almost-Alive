using System;
using EventManagement;
using GhostBehaviours;
using UnityEngine;

namespace Emotions.Controllers
{
    public class GhostEmotionController : EmotionController
    {
        protected override Vector3 DirectionOfDrop => GhostMovement.Instance.LookDirection;
        
        /// <summary>
        /// Ghost Heal after 5 orbs event
        /// </summary>
        public static event Action<float> OnGhostHeal;
        
        /// <summary>
        /// Ghost health reduction increment event  
        /// </summary>
        public static event Action<float> OnGhostFatigue;

        private void FiveSpheres()
        {
            Debug.Log("5 sphere heal");

            for (var i = 0; i < _emotions.Capacity; i++)
            {
                RemoveEmotion();
            }

            OnGhostHeal?.Invoke(+50);
            OnGhostFatigue?.Invoke(default);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_emotions.Count > 0)
                {
                    RemoveAndThrowEmotion();
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_emotions.Count == 5)
                {
                    // TODO: show ui and replace if statements
                    FiveSpheres();
                }
            }
        }

        public class MyEventParams : EventParam
        {
            public int increasedHealth;
        }
    }
}
