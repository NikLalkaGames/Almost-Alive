using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEmotionController : EmotionController
{    
    protected override Vector3 DirectionOfDrop => GetComponentInParent<GhostMovement>().LookDirection;
    
    private void Update()
    {
        // Drop emotion logic
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (_emotions.Count > 0)         // prevent IndexOutOfRangeException for empty list
            {
                DropEmotion();
                RemoveEmotion();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_emotions.Count == 5)
            {
                // show ui and replace if statements
                FiveSpheres();
            }
        }
    }
}
