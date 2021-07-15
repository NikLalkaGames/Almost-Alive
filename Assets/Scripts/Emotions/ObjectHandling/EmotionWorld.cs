using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionWorld : MonoBehaviour
{
    [SerializeField] private Emotion _emotion;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    [SerializeField] private float _idleAnimationSpeed;

    public Emotion Emotion => _emotion;

    public static Transform Spawn(Vector2 position, Emotion emotion)
    {
        Transform emotionTransform = Instantiate(EmotionAssets.Instance.pfEmotionWorld, position, Quaternion.identity).transform;

        emotionTransform.GetComponent<EmotionWorld>().SetEmotion(emotion);

        return emotionTransform;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SetEmotion(Emotion emotion)
    {
        _emotion = emotion;
        _spriteRenderer.sprite = emotion.GetSprite();
        _animator.runtimeAnimatorController = emotion.GetAnimatorController();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Consumable")) 
        {
            Debug.Log("Enter");
            other.GetComponentInChildren<EmotionController>().Handle(_emotion);
            this.gameObject.SetActive(false);
        }

    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(0, _idleAnimationSpeed / 1000, 0);
    }


}
