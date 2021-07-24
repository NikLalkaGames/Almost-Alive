using System.Collections.Generic;
using UnityEngine;

namespace Core.Helpers
{
    public static class Helpers
    {
        public static Transform GetClosestTransform(List<Transform> enemies, Transform fromThis)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = fromThis.position;
            
            foreach (Transform potentialTarget in enemies)
            {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }
            return bestTarget;
        }

        public static BoxCollider2D GetClosestCollider(List<Transform> enemies, Transform fromThis)
        {
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = fromThis.position;
            foreach (Transform t in enemies)
            {
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
            return tMin.GetComponent<BoxCollider2D>();
        }

        public static Collider2D GetClosestColliderSystemly(List<Collider2D> targets, Collider2D fromThis)
        {
            Collider2D bestTarget = null;
            ColliderDistance2D minDist = new ColliderDistance2D();
            minDist.distance = Mathf.Infinity;
            foreach (Collider2D potentialTarget in targets)
            {
                var dist = fromThis.Distance(potentialTarget);
                if (dist.distance < minDist.distance)
                {
                    minDist = dist;
                    bestTarget = potentialTarget;
                }
            }
            return bestTarget;
        }
    
        public static Vector3 GetRandomDir()
        {
            return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        }

        public static bool Reached(Vector2 currentPosition, Vector2 targetPosition) =>
            Vector2.Distance(currentPosition, targetPosition) < 1e-2;
    }
}
