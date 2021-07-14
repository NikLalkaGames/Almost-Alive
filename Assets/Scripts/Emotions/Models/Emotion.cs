using UnityEngine;

public class Emotion
{
    public EmotionColor Color { get; private set; }
    
    public Emotion(EmotionColor color) => Color = color;
}