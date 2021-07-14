﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEmotionController : EmotionController
{
    private GhostMovement _ghostMovement;

    private GhostHealth _ghostHealth;
    
    protected override Vector3 DirectionOfDrop => _ghostMovement.LookDirection;

    protected override void Start()
    {
        base.Start();
        _ghostMovement = GetComponentInParent<GhostMovement>();
        _ghostHealth = GetComponentInParent<GhostHealth>();
    }

    protected void FiveSpheres()
    {
        Debug.Log("5 sphere heal");

        for (int i = 0; i < _emotions.Capacity; i++)
        {
            RemoveEmotion();
            Destroy(_emotionHolders[i].GetChild(0).gameObject);
        }

        _ghostHealth.UpdateHealth(+50);
        _ghostHealth.IncreaseHealthReduction();
    }

    private void Update()
    {
        // Drop emotion logic
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_emotions.Count > 0)         // prevent IndexOutOfRangeException for empty list
            {
                DropEmotion();
                RemoveEmotion();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_emotions.Count == 5)
            {
                // show ui and replace if statements
                FiveSpheres();
            }
        }
    }
}