using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectNearColliders : MonoBehaviour
{
    private List<Collider2D> nearestColliders = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Consumable") || other.CompareTag("Enemy"))
        {
            if (!nearestColliders.Contains(other))
            {
                Debug.Log("Add collider of gameobject: " + other.name);
                nearestColliders.Add(other);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Consumable") || other.CompareTag("Enemy"))
        {
            if (nearestColliders.Contains(other))
            {
                Debug.Log("Remove collider of gameobject: " + other.name);
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
        foreach (var collider in nearestColliders)
        {
            transforms.Add(collider.transform);
        }

        return transforms;
    }
}
