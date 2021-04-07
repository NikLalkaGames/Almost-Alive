using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectNearestColliders : MonoBehaviour
{
    private List<Collider2D> nearestColliders = new List<Collider2D>();
    private bool forCollectibe;

    private void Start() 
    {

    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!nearestColliders.Contains(other))
        {
            if (other.CompareTag("Consumable") || other.CompareTag("Enemy") || other.CompareTag("Player"))
            {
                Debug.Log($"Add collider {other.name} to trigger zone of gameObj {this.transform.parent.name}");
                nearestColliders.Add(other);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Consumable") || other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            if (nearestColliders.Contains(other))
            {
                Debug.Log($"Remove collider {other.name} from trigger zone of gameObj {this.transform.parent.name}");
                nearestColliders.Remove(other);
            }
        }
    }

    public List<Collider2D> GetListOfTriggerColliders()
    {
        return nearestColliders;
    }

    public List<Transform> GetListOfTriggerTransforms()
    {
        List<Transform> transforms = new List<Transform>();
        Debug.Log("Colliders in trigger zone of gameobj " + this.transform.parent.name + ":");
        foreach (var collider in nearestColliders)
        {
            transforms.Add(collider.transform);
            Debug.Log("Collider: " + collider.name);
        }
        Debug.Log("----------------------------------");
        return transforms;
    }
}
