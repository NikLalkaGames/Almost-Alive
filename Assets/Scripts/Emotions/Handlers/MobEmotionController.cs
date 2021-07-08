﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobEmotionController : EmotionController
{
    # region Fields
    
    private ConsumableBehaviour _consumableHuman;

    #endregion

    #region SpriteGetters

    public Sprite GetHumanSprite(EmotionColor color)
    {
        switch (color)
        {
            default:
            case EmotionColor.blue: return Resources.Load<Sprite>("blue");
            case EmotionColor.green: return Resources.Load<Sprite>("green");
            case EmotionColor.pink: return Resources.Load<Sprite>("pink");
            case EmotionColor.purple: return Resources.Load<Sprite>("purple");
            case EmotionColor.yellow: return Resources.Load<Sprite>("yellow");
            case EmotionColor.white: return Resources.Load<Sprite>("white");
        }
    }

    public Sprite GetDeadSprite(EmotionColor color)
    {
        switch (color)
        {
            default:
            case EmotionColor.blue: return Resources.Load<Sprite>("blue_dead");
            case EmotionColor.green: return Resources.Load<Sprite>("green_dead");
            case EmotionColor.pink: return Resources.Load<Sprite>("pink_dead");
            case EmotionColor.purple: return Resources.Load<Sprite>("purple_dead");
            case EmotionColor.yellow: return Resources.Load<Sprite>("yellow_dead");
            case EmotionColor.white: return Resources.Load<Sprite>("white_dead");
        }
    }

    # endregion

    private void Awake()
    {
        _consumableHuman = GetComponentInParent<ConsumableBehaviour>();
        
        if (_consumableHuman != null)
        {
            onHandle += DefineSkinColor;
            ConsumableBehaviour.OnKilled += DropEmotionsAfterDeath;
        }
    }

    private void Start()
    {
        Handle(_consumableHuman.HumanColor);
    }
    
    public void DropEmotionsAfterDeath(EmotionColor emotionColor)
    {
        Debug.Log("Drop Emotions After Death");
        for (int i = 0; i < transform.childCount; i++)
        {
            var emotionToDrop = RemoveEmotion();
            Destroy(transform.GetChild(i).gameObject);      // destroy internal emotion
            DropEmotion(emotionToDrop.EmotionColor);
            Debug.Log(Emotions.Length);
        }
        _globalAngle = -180;
    }

    public void DefineSkinColor()
    {
        if (_emotions.Length <= 1)
        {
            _consumableHuman.HumanSprite = GetHumanSprite(_consumableHuman.HumanColor);
            _consumableHuman.DeadSprite = GetDeadSprite(_consumableHuman.HumanColor);
            // change animation controller of human
        }
        else
        {
            _consumableHuman.HumanColor = EmotionColor.white;
            _consumableHuman.HumanSprite = GetHumanSprite(_consumableHuman.HumanColor);
            _consumableHuman.DeadSprite = GetDeadSprite(_consumableHuman.HumanColor);
            // change animation controller of human
        }
    }

}
