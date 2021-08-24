using UnityEngine;

namespace Core.CollisionDetection.Interfaces
{
    internal interface IColliderDetector2D
    {
        void OnTriggerEnter2D(Collider2D other);
        void OnTriggerExit2D(Collider2D other);
    }
}
