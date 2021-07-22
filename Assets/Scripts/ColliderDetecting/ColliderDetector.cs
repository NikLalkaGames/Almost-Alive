using System;
using System.Collections;
using ColliderDetecting.Interfaces;
using UnityEngine;

namespace ColliderDetecting
{
    public abstract class ColliderDetector : MonoBehaviour, IColliderDetector
    {
        protected IEnumerator _coroutine;

        public abstract void OnTriggerEnter2D(Collider2D other);
        public abstract void OnTriggerExit2D(Collider2D other);

    }    
}