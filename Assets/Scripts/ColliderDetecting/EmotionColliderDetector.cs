using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmotionColliderDetector : ColliderDetector
{
    public override void OnTriggerEnter2D(Collider2D other) 
    {   
        _coroutine = MagnetTo(other.transform, transform);

        if (other.CompareTag("Emotion") && (!_emotionController.Emotions.Exists(e => e.Color == other.GetComponent<EmotionWorld>().Emotion.Color) ))
        {
            StartCoroutine( _coroutine ); //fix multiple TriggerEnter
        }
    }

    public override void OnTriggerExit2D(Collider2D other) 
    {
        Debug.Log("StopCoroutine");
        StopCoroutine( _coroutine );
    }

    protected IEnumerator MagnetTo(Transform magnetFrom, Transform magnetTo)
    {
        float pickUpSpeed = 0f;
        while (!Helper.Reached(magnetFrom.position, magnetTo.position))
        {
            yield return new WaitForEndOfFrame();
            pickUpSpeed = 1.5f - Vector2.Distance(magnetFrom.position, magnetTo.position);
            magnetFrom.position = Vector2.MoveTowards(magnetFrom.position, magnetTo.position, pickUpSpeed * Time.deltaTime);   //transform from player position to 
        }
    }
}
