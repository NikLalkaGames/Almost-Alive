using System;
using System.Collections;
using Emotions.Controllers;
using UnityEngine;
using static Core.Helpers.Helpers;

namespace Core.CollisionDetecting
{
    public class EmotionColliderDetector : ColliderDetector
    {
        protected EmotionController _emotionController;
        
        private void Awake() 
        {
            _emotionController = transform.parent.GetComponentInChildren<EmotionController>();    
        }
        
        public override void OnTriggerEnter2D(Collider2D other) 
        {   
            _coroutine = MagnetTo(other.transform, transform);

            if (other.CompareTag("Emotion") && 
                ( !_emotionController.EmotionExists(other.GetComponent<EmotionWorld>().Emotion) ) )
            {
                StartCoroutine( _coroutine );   // fix multiple TriggerEnter
            }
        }

        public override void OnTriggerExit2D(Collider2D other) 
        {
            Debug.Log("StopCoroutine");
            StopCoroutine( _coroutine );
        }

        private static IEnumerator MagnetTo(Transform magnetFrom, Transform magnetTo)
        {
            var magnetFromPosition = magnetFrom.position;   // cached
            var magnetToPosition = magnetTo.position;       // cached

            while (!Reached(magnetFrom.position, magnetTo.position))
            {
                yield return new WaitForEndOfFrame();

                var pickUpSpeed = 1.8f - Vector2.Distance(magnetFromPosition, magnetToPosition);
                
                magnetFromPosition = Vector2.MoveTowards(magnetFromPosition, magnetToPosition, pickUpSpeed * Time.deltaTime);
                magnetFrom.position = magnetFromPosition;
            }
        }
    }
    
}
