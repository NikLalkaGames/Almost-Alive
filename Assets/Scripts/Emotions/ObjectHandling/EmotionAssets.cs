using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace Emotions.ObjectHandling
{
    public class EmotionAssets : MonoBehaviour
    {
        public static EmotionAssets Instance { get; private set; }
    
        [System.Serializable]
        public class EmotionAsset
        {
            public Sprite sprite;
            public RuntimeAnimatorController animController;
            public GameObject particleObject;
        }
    

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public Transform pfEmotionWorld;

        public List<EmotionAsset> assets = new List<EmotionAsset>(5);
    }
}
