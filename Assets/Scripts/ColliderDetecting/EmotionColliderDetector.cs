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

    protected IEnumerator MagnetTo(Transform MagnetObject, Transform MagnetToObject)
    {
        var speed = 0f;
        while (!Helper.Reached(MagnetObject.position, MagnetToObject.position))
        {
            yield return new WaitForEndOfFrame();
            speed += 0.01f;
            MagnetObject.position = Vector2.MoveTowards(MagnetObject.position, MagnetToObject.position, speed * Time.deltaTime);   //transform from player position to 
        }
    }
}
