using UnityEngine;

namespace Core.CollisionDetection.Interfaces
{
    public interface IColliderDetector
    {
        void OnTriggerEnter(Collider other);

        void OnTriggerExit(Collider other);
    }
}