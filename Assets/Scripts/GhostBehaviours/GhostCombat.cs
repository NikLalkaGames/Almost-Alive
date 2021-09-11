using System.Collections;
using Core.CollisionDetection.HeroColliderDetectors;
using Core.Helpers;
using UnityEngine;

namespace GhostBehaviours
{
    public class GhostCombat : MonoBehaviour
    {
        [SerializeField] private MobColliderDetector _ghostColliderDetector;

        [SerializeField] private Animator _ghostAnimator;

        public Transform _destTransform;
        
        private void Start()
        {
            // can me initialization of some components
            
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                var sourceTransform = new GameObject().transform;
                StartCoroutine(AttackCoroutine(sourceTransform, _destTransform));
            }
        }

        private IEnumerator AttackCoroutine(Transform sourceTransform, Transform destTransform)
        {
            while (!Helpers.Reached(sourceTransform.position, destTransform.position))
            {
                yield return new WaitForEndOfFrame();
                transform.position = Vector3.MoveTowards(sourceTransform.position, destTransform.position, Time.deltaTime * 2f);
            }
        }
    }
    
    
}