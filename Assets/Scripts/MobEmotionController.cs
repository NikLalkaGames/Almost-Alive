using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobEmotionController : EmotionController
{
    public void DropEmotionsAfterDeath(EmotionColor emotionColor)
    {
        Debug.Log("Drop Emotions After Death");
        for (int i = 0; i < transform.childCount; i++)
        {
            var emotionToDrop = RemoveEmotion();
            Destroy(transform.GetChild(i).gameObject);      // destroy internal emotion
            DropEmotion(this.gameObject.transform.position, this.DirectionOfDrop, emotionToDrop.EmotionColor);
            Debug.Log(Emotions.Count);
        }
        globalAngle = -180;
    }
}
