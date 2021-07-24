using Core.CollisionDetecting;
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
            
        // TODO: replace on event-callback interaction between objects (OnEmotionMagnetFieldEnters) (emotionController with emotionWorld)
        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Emotion") 
                && ( !emotionController.EmotionExists(other.GetComponent<EmotionWorld>().Emotion) ) )     
            {
                _coroutine = MagnetTo(other.transform, transform, colliderRadius);
                StartCoroutine( _coroutine );   // fix multiple TriggerEnter
            }
        }

        public override void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Stop magnet coroutine");
            StopCoroutine(_coroutine);
        }
    }
    
}
