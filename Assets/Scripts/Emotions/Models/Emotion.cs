using Emotions.ObjectHandling;
using UnityEngine;

namespace Emotions.Models
{
    [System.Serializable]
    public class Emotion
    {
        [SerializeField] 
        private EmotionColor _emotionColor;
        public EmotionColor Color => _emotionColor;
    
        public Emotion(EmotionColor color) => _emotionColor = color;

        public Sprite GetSprite()
        {
            switch (Color)
            {
                case EmotionColor.blue: return EmotionAssets.Instance.assets[0].sprite;
                case EmotionColor.green: return EmotionAssets.Instance.assets[1].sprite;
                case EmotionColor.pink: return EmotionAssets.Instance.assets[2].sprite;
                case EmotionColor.purple: return EmotionAssets.Instance.assets[3].sprite;
                case EmotionColor.yellow: return EmotionAssets.Instance.assets[4].sprite;
            
                default: throw new System.ArgumentException("The Method received wrong color");
            }
        }

        public RuntimeAnimatorController GetAnimatorController()
        {
            switch (Color)
            {
                case EmotionColor.blue: return EmotionAssets.Instance.assets[0].animController;
                case EmotionColor.green: return EmotionAssets.Instance.assets[1].animController;
                case EmotionColor.pink: return EmotionAssets.Instance.assets[2].animController;
                case EmotionColor.purple: return EmotionAssets.Instance.assets[3].animController;
                case EmotionColor.yellow: return EmotionAssets.Instance.assets[4].animController;
            
                default: throw new System.ArgumentException("The Method received wrong color");
            }
        }


    }
}