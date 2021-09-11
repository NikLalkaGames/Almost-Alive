using System;
using Core.CollisionDetection.Interfaces;
using Core.Health.Interfaces;
using Enemies;
using UnityEngine;

namespace Core.CollisionDetection.HeroColliderDetectors
{
    public class MobColliderDetector : MonoBehaviour, IColliderDetector
    {
        #region Fields

        [SerializeField] private float damageAmount;
        
        #endregion
        
        #region IColliderDetector implementation
        
        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable mob))
            {
                mob.TryToDamage(damageAmount);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            
        }
        
        #endregion
    }
}