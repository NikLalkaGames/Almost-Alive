using UnityEngine;

namespace Core.CollisionDetecting.Interfaces
{
    internal interface IColliderDetector
    {
        void OnTriggerEnter2D(Collider2D other);
        void OnTriggerExit2D(Collider2D other);
    }
}
