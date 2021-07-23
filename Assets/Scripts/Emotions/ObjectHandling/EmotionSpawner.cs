using Emotions.Models;
using UnityEngine;

namespace Emotions.ObjectHandling
{
    public class EmotionSpawner : MonoBehaviour
    {
        public Emotion emotionToSpawn;

        private void Start()
        {
            EmotionWorld.TakeFromPoolAndPlace(transform.position, emotionToSpawn);
            Destroy(this.gameObject);
        }
    }
}
