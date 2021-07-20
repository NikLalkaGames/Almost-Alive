using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ColliderDetector : MonoBehaviour, IColliderDetector
{
    protected EmotionController _emotionController;

    protected IEnumerator _coroutine;
    private void Awake() 
    {
        _emotionController = transform.parent.GetComponentInChildren<EmotionController>();    
    }


    public abstract void OnTriggerEnter2D(Collider2D other);
    public abstract void OnTriggerExit2D(Collider2D other);
}
