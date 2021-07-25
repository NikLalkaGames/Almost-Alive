using System;
using System.Collections.Generic;
using Emotions.Controllers;
using Emotions.ObjectHandling;
using UnityEngine;
using static Emotions.Controllers.EmotionController;

namespace Core.CollisionDetection
{
    public class EmotionColliderDetector : ColliderDetector
    {
        [SerializeField] private EmotionController emotionController;

        public float colliderRadius;

        private List<Transform> _caughtEmotionTransforms = new List<Transform>();

        private void LateUpdate()
        {
            foreach (var emotionTransform in _caughtEmotionTransforms)
            {
                MagnetStep(emotionTransform, transform, colliderRadius);
            }
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            var otherTransform = other.transform;
            
            if (other.CompareTag("Emotion") && !_caughtEmotionTransforms.Contains(otherTransform)
                && !emotionController.EmotionExists(other.GetComponent<EmotionWorld>().Emotion) )
            {
                _caughtEmotionTransforms.Add(otherTransform);
            }
        }

        public override void OnTriggerExit2D(Collider2D other)
        {
            var otherTransform = other.transform;
            
            if (_caughtEmotionTransforms.Contains(otherTransform))
            {
                _caughtEmotionTransforms.Remove(otherTransform);
            }
        }
        
    }
    
}
