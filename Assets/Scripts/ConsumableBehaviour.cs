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
        GameObject.Find("Spawner").GetComponent<Spawner>().getKilled = true;
        GameObject.Find("Spawner").GetComponent<Spawner>().killedColor = emotionColor;
        GetComponent<SpriteRenderer>().sprite = deadSprite;
        // GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<HumanController>().enabled = false;
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAA!!!");
    }
}
