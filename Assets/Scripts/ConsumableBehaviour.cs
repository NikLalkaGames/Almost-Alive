using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConsumableBehaviour : MonoBehaviour
{
    private MobEmotionController emotionController;

    private SpriteRenderer humanSpriteRenderer;

    public Sprite deadSprite;
    
    public EmotionColor humanColor;
    
    private Spawner spawner;
    
    [System.Serializable]
    public class ActionKill : UnityEvent<EmotionColor> {}
    
    public ActionKill onKilled;

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

    private void InitEmotion()
    {
        Debug.Log("Init human emotion");
        emotionController.Handle(humanColor);     // create initial emotion
    }

    public void DefineColorByEmotion()
    {
        if (emotionController.Emotions.Count <= 1)
        {
            humanColor = emotionController.Emotions[0].EmotionColor;    // save internal value
            humanSpriteRenderer.sprite = GetHumanSprite(humanColor);
            deadSprite = GetDeadSprite(humanColor);
            // change animation controller of human
        }
        else
        {
            Debug.Log("Number of emotions in emotionContorller: " + emotionController.Emotions.Count); 
            humanColor = EmotionColor.white;            // save internal value
            humanSpriteRenderer.sprite = GetHumanSprite(humanColor);
            deadSprite = GetDeadSprite(humanColor);
            // change animation controller of human
        }
    }
    
    private void Start()
    {
        humanSpriteRenderer = GetComponent<SpriteRenderer>();
        emotionController = GetComponentInChildren<MobEmotionController>();

        InitEmotion();
        // DefineColorByEmotion();

        if ( ( spawner = GameObject.Find("Spawner").GetComponent<Spawner>() ) != null)
        {
            onKilled.AddListener(spawner.GenerateMatchingHuman);
            onKilled.AddListener(emotionController.DropEmotionsAfterDeath);
        }
        else
        {
            throw new System.Exception();
        }
    }

    public void Kill()
    {
        if (onKilled != null)
            onKilled.Invoke(humanColor);
            
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = deadSprite;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<HumanController>().enabled = false;
        this.enabled = false;
    }

    private void OnDisable()
    {
        onKilled.RemoveListener(spawner.GenerateMatchingHuman);
        onKilled.RemoveListener(emotionController.DropEmotionsAfterDeath);
    }
}
