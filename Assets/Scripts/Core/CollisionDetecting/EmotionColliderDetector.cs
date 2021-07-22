using System.Collections;
using UnityEngine;

namespace Core.CollisionDetecting
{
    public class EmotionColliderDetector : ColliderDetector
    {
        public override void OnTriggerEnter2D(Collider2D other) 
        {   
            coroutine = MagnetTo(other.transform, transform);

            if (other.CompareTag("Emotion") )
            {
                StartCoroutine( coroutine ); //fix multiple TriggerEnter
            }
        }

        public override void OnTriggerExit2D(Collider2D other) 
        {
            Debug.Log("StopCoroutine");
            StopCoroutine( coroutine );
        }

        private static IEnumerator MagnetTo(Transform magnetFrom, Transform magnetTo)
        {
            var magnetFromPosition = magnetFrom.position;   // cached
            var magnetToPosition = magnetTo.position;       // cached

            while (!Helpers.Helpers.Reached(magnetFrom.position, magnetTo.position))
            {
                yield return new WaitForEndOfFrame();

                var pickUpSpeed = 1.5f - Vector2.Distance(magnetFromPosition, magnetToPosition);
                
                magnetFromPosition = Vector2.MoveTowards(magnetFromPosition, magnetToPosition, pickUpSpeed * Time.deltaTime);
                magnetFrom.position = magnetFromPosition;
            }
        }
    }
    
}
