using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionSpawner : MonoBehaviour
{
    public Emotion emotionToSpawn;

    void Start()
    {
        EmotionWorld.Spawn(transform.position, emotionToSpawn);
        Destroy(this.gameObject);
    }
}
