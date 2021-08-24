using System.Collections.Generic;
using Core.CollisionDetection.Interfaces;
using Emotions.Controllers;
using Emotions.Object;
using UnityEngine;
using static Core.Helpers.Helpers;

namespace Emotions.Interaction
{
    public class EmotionMagnet : MonoBehaviour, IColliderDetector
    {
        #region Fields
        
        public float colliderRadius;

        private List<Emotion> _caughtEmotions = new List<Emotion>();
        
        [SerializeField] private EmotionController emotionController;

        private Transform _emotionHolder;
        
        #endregion
        
        #region Awake

        private void Awake()
        {
            _emotionHolder = emotionController.transform;
        }

        #endregion
        
        #region Emotion magnet logic

        private static bool EmotionExistsIn(List<Emotion> emotions, Emotion enteredEmotion) =>
            emotions.Exists(e => e.Color == enteredEmotion.Color);

        private void MagnetStep(Emotion emotion, Transform magnetTo)
        {
            var emotionTransform = emotion.transform;
            
            var emotionPosition = emotionTransform.position;
            
            var magnetToPosition = magnetTo.position;

            var pickUpSpeed =  colliderRadius - Vector3.Distance(emotionPosition, magnetToPosition);

            emotionPosition = Vector3.MoveTowards(emotionPosition, magnetToPosition, 1.2f * pickUpSpeed * Time.deltaTime);
            emotionTransform.position = emotionPosition;
            
            if (Reached(emotionPosition, magnetToPosition))
            {
                _caughtEmotions.Remove(emotion);
                
                emotionController.Handle(emotion);
            }
        }
        
        private void LateUpdate()
        {
            for (var i = 0; i < _caughtEmotions.Count; i++)
            {
                MagnetStep(_caughtEmotions[i], _emotionHolder);
            }
        }
        
        #endregion
        
        #region IColliderDetector Implementation

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Emotion emotion))
            {
                Debug.Log("Enter in external trigger of emotion controller");
                
                if (!EmotionExistsIn(_caughtEmotions, emotion) 
                    && !EmotionExistsIn(emotionController.Emotions, emotion))
                {
                    _caughtEmotions.Add(emotion);
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Emotion emotion))
            {
                Debug.Log("Exit from external trigger of emotion controller");
                
                if (EmotionExistsIn(_caughtEmotions, emotion))
                {
                    _caughtEmotions.Remove(emotion);
                }
            }
        }
        
        #endregion
    }
}
