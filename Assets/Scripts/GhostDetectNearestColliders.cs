using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostDetectNearestColliders : DetectNearestColliders
{

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!_nearestColliders.Contains(other))
        {
            if (other.CompareTag("Emotion"))
            {
                //Debug.Log($"Add collider {other.name} to trigger zone of gameObj {this.transform.parent.name}");
                _nearestColliders.Add(other);

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

            }
        }
    }
}