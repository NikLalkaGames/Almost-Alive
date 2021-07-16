using UnityEngine;

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
            default:
            case EmotionColor.blue: return EmotionAssets.Instance.assets[0].sprite;
            case EmotionColor.green: return EmotionAssets.Instance.assets[1].sprite;
            case EmotionColor.pink: return EmotionAssets.Instance.assets[2].sprite;
            case EmotionColor.purple: return EmotionAssets.Instance.assets[3].sprite;
            case EmotionColor.yellow: return EmotionAssets.Instance.assets[4].sprite;
        }
    }

    public RuntimeAnimatorController GetAnimatorController()
    {
        switch (Color)
        {
            default:
            case EmotionColor.blue: return EmotionAssets.Instance.assets[0].animController;
            case EmotionColor.green: return EmotionAssets.Instance.assets[1].animController;
            case EmotionColor.pink: return EmotionAssets.Instance.assets[2].animController;
            case EmotionColor.purple: return EmotionAssets.Instance.assets[3].animController;
            case EmotionColor.yellow: return EmotionAssets.Instance.assets[4].animController;
        }
    }


}