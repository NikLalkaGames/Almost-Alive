using System.Collections.Generic;
using Core.CollisionDetection.Interfaces;
using Emotions.Controllers;
using Emotions.Models;
using Emotions.Object;
using UnityEngine;
using static Emotions.Controllers.EmotionController;

namespace Core.CollisionDetection.HeroColliderDetectors
{
    public class EmotionColliderDetector : MonoBehaviour, IColliderDetector
    {
        #region Fields
        
        public float colliderRadius;
        
        private Dictionary<EmotionColor, Transform> _caughtEmotionTransforms
            = new Dictionary<EmotionColor, Transform>();
        
        [SerializeField] private EmotionController emotionController;

        #endregion
        
        #region Monobehaviour Emotion Magnet
        
        private void LateUpdate()
        {
            foreach (var emotion in _caughtEmotionTransforms)
            {
                MagnetStep(emotion.Value, transform, colliderRadius);
            }
        }
        
        #endregion
        
        #region IColliderDetector2D Implementation

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Emotion emotion))
            {
                var otherTransform = other.transform;
                if (!_caughtEmotionTransforms.ContainsKey(emotion.Color) && !emotionController.EmotionExists(emotion))
                {
                    _caughtEmotionTransforms.Add(emotion.Color, otherTransform);
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Emotion emotion))
            {
                Debug.Log("Exit from external trigger of player obj");
                
                var otherTransform = other.transform;
                if (_caughtEmotionTransforms.ContainsKey(emotion.Color)
                    && _caughtEmotionTransforms.ContainsValue(otherTransform))
                {
                    _caughtEmotionTransforms.Remove(emotion.Color);
                }
            }
        }
        
        #endregion
    }
}
