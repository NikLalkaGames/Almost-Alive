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

    DetectNearestColliders TriggerZone;

    private void Start() 
    {
        TriggerZone = GetComponent<DetectNearestColliders>();
    }

    private void Update()
    {
        if (TriggerZone.NearestColliders.Count != 0)
        {
            ListenInteractByKeyPress();
        }
    }

    private void ListenInteractByKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            var nearestTransform = Helper.GetClosestTransform(TriggerZone.GetListOfTriggerTransforms(), transform);
            if (nearestTransform.CompareTag("Consumable"))
            {
                Debug.Log("Hitted : " + nearestTransform.name);
                ConsumableBehaviour littleMan = nearestTransform.GetComponent<ConsumableBehaviour>();
                if (littleMan != null)
                {
                    littleMan.Kill();
                }
            }
            else if(nearestTransform.CompareTag("Enemy"))
            {
                Debug.Log("This is enemy");
            }
        }
    }
    
    // kill last collider that entered in trigger zone
/*  if (TriggerZone.GetListOfNearestColliders().Count != 0)
    {
        var nearestColliders = TriggerZone.GetListOfNearestColliders();
        ListenInteractByKeyPress(nearestColliders[nearestColliders.Count - 1]);
    } */

    // if need to switch to mouse button (Input.GetButtonDown("Fire1"))
}
