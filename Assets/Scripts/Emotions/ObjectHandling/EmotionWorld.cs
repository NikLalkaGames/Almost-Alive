using System;
using Emotions.Controllers;
using Emotions.Models;
using Emotions.ObjectHandling;
using UnityEngine;


public class EmotionWorld : MonoBehaviour
{
    # region Static

    public static EmotionWorld TakeFromPoolAndPlace(Vector2 positionToSpawn, Emotion emotion)
    {
        EmotionWorld newEmotoin = EmotionObjectPool.Instance.GetEmotion();

        if (newEmotoin != null)
        {
            newEmotoin.gameObject.SetActive(true);
            newEmotoin.Init(emotion);
            newEmotoin.transform.position = positionToSpawn;
            return newEmotoin;
        }
            
        throw new NullReferenceException("Couldn't get a new emotion");
        // Transform emotionTransform = Instantiate(EmotionAssets.Instance.pfEmotionWorld, position, Quaternion.identity).transform
    }

    #endregion

    # region Fields

    [SerializeField] private Emotion _emotion;
    
    private SpriteRenderer _spriteRenderer;
    
    private Animator _animator;

    private BoxCollider2D _internalCollider;
    
    [SerializeField] private float _idleAnimationSpeed;

    
    [System.NonSerialized] public EmotionWorld next;    
    
    
    public Emotion Emotion => _emotion;

    #endregion

    #region Events

    /// <summary>
    /// Configure deactivated emotion callback invoker after emotion removing from emotion world       
    /// </summary>
    public static event Action<EmotionWorld> OnDeactivate;

    #endregion

    #region Methods

    private void Awake()
    {
        _internalCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        
    }

    public void Init(Emotion emotion)
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
            var isHandled = other.GetComponentInChildren<EmotionController>().Handle(_emotion);

            if (isHandled)
            {
                this.gameObject.SetActive(false);
                OnDeactivate?.Invoke(this);
            }
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(0, _idleAnimationSpeed / 1000, 0);
    }

    # endregion
   
}
