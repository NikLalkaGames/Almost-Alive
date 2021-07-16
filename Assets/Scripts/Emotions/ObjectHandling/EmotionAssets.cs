using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionAssets : MonoBehaviour
{
    public static EmotionAssets Instance { get; private set; }
    
    [System.Serializable]
    public class EmotionAsset
    {
        public Sprite sprite;
        public RuntimeAnimatorController animController;
    }
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public Transform pfEmotionWorld;

    public List<EmotionAsset> assets = new List<EmotionAsset>(5);
}
