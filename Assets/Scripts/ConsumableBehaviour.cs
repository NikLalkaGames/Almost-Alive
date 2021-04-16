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
            case EmotionColor.blue: return Resources.Load("blue") as Sprite;
            case EmotionColor.green: return Resources.Load("green") as Sprite;
            case EmotionColor.pink: return Resources.Load("pink") as Sprite;
            case EmotionColor.purple: return Resources.Load("purple") as Sprite;
            case EmotionColor.yellow: return Resources.Load("yellow") as Sprite;
            case EmotionColor.white: return Resources.Load("white") as Sprite;
        }
    }

    public Sprite GetDeadSprite(EmotionColor color)
    {
        switch (color)
        {
            default:
            case EmotionColor.blue: return Resources.Load("blue_dead") as Sprite;
            case EmotionColor.green: return Resources.Load("green_dead") as Sprite;
            case EmotionColor.pink: return Resources.Load("pink_dead") as Sprite;
            case EmotionColor.purple: return Resources.Load("purple_dead") as Sprite;
            case EmotionColor.yellow: return Resources.Load("yellow_dead") as Sprite;
            case EmotionColor.white: return Resources.Load("white_dead") as Sprite;
        }
    }

    private void InitEmotion()
    {
        Debug.Log("Init");
        emotionController.Handle(humanColor);     // create initial emotion
    }

    public void DefineColorByEmotion()
    {
        if (emotionController.Emotions.Count == 1)
        {
            humanColor = emotionController.Emotions[0].EmotionColor;    // save internal value
            humanSpriteRenderer.sprite = GetHumanSprite(humanColor);
            deadSprite = GetDeadSprite(humanColor);
            // change animation controller of human
        }
        else if (emotionController.Emotions.Count > 1)
        {
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
