using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableBehaviour : MonoBehaviour
{
    private EmotionController emotion;
    public Sprite deadSprite;
    // Start is called before the first frame update
    public EmotionColor emotionColor;

    private void Start()
    {
        emotion = GameObject.Find("PlayerGhost").GetComponent<EmotionController>();
    }

    public void Kill()
    {
        Debug.Log("KILL!");
        
        emotion.SpawnEmotion(transform.position + Vector3.up * 0.2f, emotionColor);
        if ( GameObject.Find("Spawner").GetComponent<Spawner>() != null)
        {
            var spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
            spawner.getKilled = true;
            spawner.killedColor = emotionColor;
        }
        else
        {
            Debug.Log("Null reference exception");
        }


        GetComponent<SpriteRenderer>().sprite = deadSprite;
        // GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<HumanController>().enabled = false;
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAA!!!");
    }
}
