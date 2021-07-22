using UnityEngine;

namespace ColliderDetecting.Interfaces
{
    interface IColliderDetector
    {
        void OnTriggerEnter2D(Collider2D other);
        void OnTriggerExit2D(Collider2D other);
    }
}
