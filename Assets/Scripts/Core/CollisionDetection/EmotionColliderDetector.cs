using System;
using System.Collections.Generic;
using Emotions.Controllers;
using Emotions.Models;
using Emotions.Object;
using UnityEngine;
using static Emotions.Controllers.EmotionController;

namespace Core.CollisionDetection
{
    public class EmotionColliderDetector : ColliderDetector
    {
        [SerializeField] private EmotionController emotionController;

        public float colliderRadius;

        private Dictionary<EmotionColor, Transform>
            _caughtEmotionTransforms = new Dictionary<EmotionColor, Transform>();

        private void LateUpdate()
        {
            foreach (var emotion in _caughtEmotionTransforms)
            {
                MagnetStep(emotion.Value, transform, colliderRadius);
            }
        }
        
        public override void OnTriggerEnter2D(Collider2D other)
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

        public override void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Emotion emotion))
            {
                var otherTransform = other.transform;
                if (_caughtEmotionTransforms.ContainsKey(emotion.Color)
                    && _caughtEmotionTransforms.ContainsValue(otherTransform))
                {
                    _caughtEmotionTransforms.Remove(emotion.Color);
                }
            }
        }
        
        
    }
}
