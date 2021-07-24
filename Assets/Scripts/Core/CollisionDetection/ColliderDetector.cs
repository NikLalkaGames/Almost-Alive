using System.Collections;
using Core.CollisionDetecting.Interfaces;
using UnityEngine;

namespace Core.CollisionDetection
{
    public abstract class ColliderDetector : MonoBehaviour, IColliderDetector
    {
        protected IEnumerator _coroutine;
        
        public abstract void OnTriggerEnter2D(Collider2D other);
        public abstract void OnTriggerExit2D(Collider2D other);
    }    
}