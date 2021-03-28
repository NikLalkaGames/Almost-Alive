using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConsumableBehaviour : MonoBehaviour
{
    private EmotionController emotion;
    public Sprite deadSprite;
    public EmotionColor emotionColor;
    private Spawner spawner;
    
    [System.Serializable]
    public class ActionKill : UnityEvent<EmotionColor> {}
    public ActionKill onKilled;
    
    private void Start()
    {
        emotion = GameObject.Find("PlayerGhost").GetComponent<EmotionController>();
        if ( GameObject.Find("Spawner").GetComponent<Spawner>() != null)
        {
            spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
            onKilled.AddListener(spawner.GenerateMatchingHuman);
        }
        else
        {
            throw new System.Exception();
        }
    }

    public void Kill()
    {
        if (onKilled != null)
            onKilled.Invoke(emotionColor);
            
        emotion.SpawnEmotion(transform.position + Vector3.up * 0.2f, emotionColor);
        GetComponent<SpriteRenderer>().sprite = deadSprite;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<HumanController>().enabled = false;
        this.enabled = false;
    }

    private void OnDisable()
    {
        onKilled.RemoveListener(spawner.GenerateMatchingHuman);
    }
}
