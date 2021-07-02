using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetectNearestColliders : MonoBehaviour
{
    private List<Collider2D> _nearestColliders = new List<Collider2D>();

    public static event System.Action OnColliderDetectorEnter;
    public static event System.Action OnColliderDetectorExit;

    public List<Collider2D> NearestColliders => _nearestColliders;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!_nearestColliders.Contains(other))
        {
            if (other.CompareTag("Consumable") || other.CompareTag("Enemy") || other.CompareTag("Player"))
            {
                //Debug.Log($"Add collider {other.name} to trigger zone of gameObj {this.transform.parent.name}");
                _nearestColliders.Add(other);

                OnColliderDetectorEnter?.Invoke();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Consumable") || other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            if (_nearestColliders.Contains(other))
            {
                //Debug.Log($"Remove collider {other.name} from trigger zone of gameObj {this.transform.parent.name}");
                _nearestColliders.Remove(other);

                OnColliderDetectorExit?.Invoke();
            }
        }
    }

    public List<Transform> GetListOfTriggerTransforms() => _nearestColliders.Select(x => x.transform).ToList();
}
