using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CombatSystem : MonoBehaviour
{
    // not even work
    /*     private void OnTriggerStay2D(Collider2D other) 
        {
            Debug.Log("Inside trigger zone: " + other.name);
            ListenInteractByKeyPress(other);
        } */

    DetectNearColliders TriggerZone;

    private void Start() 
    {
        TriggerZone = GetComponent<DetectNearColliders>();
    }

    private void Update()
    {
        if (TriggerZone.GetListOfTriggerColliders().Count != 0)
        {
            ListenInteractByKeyPress();
        }
    }

    private void ListenInteractByKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var nearestCollider = GetClosestEnemy(TriggerZone.GetListOfTriggerTransforms());
            if (nearestCollider.CompareTag("Consumable"))
            {
                Debug.Log("Collider of : " + nearestCollider.name);
                ConsumableBehaviour littleMan = nearestCollider.GetComponent<ConsumableBehaviour>();
                if (littleMan != null)
                {
                    littleMan.Kill();
                }
            }
            else if(nearestCollider.CompareTag("Enemy"))
            {
                Debug.Log("This is enemy");
            }
        }
    }

    BoxCollider2D GetClosestEnemy(List<Transform> enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
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
    
    // kill last collider that entered in trigger zone
    /*     if (TriggerZone.GetListOfNearestColliders().Count != 0)
    {
        var nearestColliders = TriggerZone.GetListOfNearestColliders();
        ListenInteractByKeyPress(nearestColliders[nearestColliders.Count - 1]);
    } */

    // if need to switch to mouse button (Input.GetButtonDown("Fire1"))
}
