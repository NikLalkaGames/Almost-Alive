public class Emotion
{
    private EmotionColor _emotionColor;

    public Emotion(EmotionColor ec) => _emotionColor = ec;

    public EmotionColor EmotionColor { get => _emotionColor; set => _emotionColor = value; }
}