using System;
using System.Collections;
using UnityEngine;
using static Core.Helpers.Helpers;

namespace Core.CollisionDetecting
{
    public class EmotionColliderDetector : ColliderDetector
    {
        #region Static
        
        /// <summary>
        /// Event for check emotion existence after OnTriggerEnter event
        /// </summary>
        public static event Func<Collider2D, bool> OnEmotionCheck;

        public static bool EmotionExists(Collider2D other)
        {
            return OnEmotionCheck.Invoke(other);
        }
        
        #endregion
        
        public override void OnTriggerEnter2D(Collider2D other) 
        {   
            _coroutine = MagnetTo(other.transform, transform);

            if (other.CompareTag("Emotion") && !EmotionExists(other))
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

                var pickUpSpeed = 2.0f - Vector2.Distance(magnetFromPosition, magnetToPosition);
                
                magnetFromPosition = Vector2.MoveTowards(magnetFromPosition, magnetToPosition, pickUpSpeed * Time.deltaTime);
                magnetFrom.position = magnetFromPosition;
            }
        }
    }
    
}
