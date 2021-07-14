using UnityEngine;

public class Emotion
{
    public EmotionColor Color { get; set; }
    
    public Emotion(EmotionColor ec) => Color = ec;
}