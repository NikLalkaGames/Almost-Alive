using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobEmotionController : EmotionController
{
    // Add later gradient functionality 

    // call before consumable behaviour Start Method
    private void Awake()
    {
        var consumableBehaviour = GetComponentInParent<ConsumableBehaviour>();
        if (consumableBehaviour != null)
            onHandle += consumableBehaviour.DefineColorByEmotion;
    }
    
    public void DropEmotionsAfterDeath(EmotionColor emotionColor)
    {
        Debug.Log("Drop Emotions After Death");
        for (int i = 0; i < transform.childCount; i++)
        {
            var emotionToDrop = RemoveEmotion();
            Destroy(transform.GetChild(i).gameObject);      // destroy internal emotion
            DropEmotion(this.gameObject.transform.position, Vector3.zero, emotionToDrop.EmotionColor);
            Debug.Log(Emotions.Count);
        }
        globalAngle = -180;
    }

}
