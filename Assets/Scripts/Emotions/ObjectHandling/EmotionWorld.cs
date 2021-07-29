using System;
using Emotions.Controllers;
using Emotions.Models;
using Unity.Mathematics;
using UnityEngine;

namespace Emotions.ObjectHandling
{
    public class EmotionWorld : MonoBehaviour
    {
        # region Static

        public static EmotionWorld TakeFromPoolAndPlace(Vector2 positionToSpawn, Emotion emotion)
        {
            var newEmotion = EmotionObjectPool.Instance.GetEmotion();

            if (newEmotion != null)
            {
                newEmotion.gameObject.SetActive(true);
                newEmotion.Init(emotion);
                newEmotion.transform.position = positionToSpawn;
                newEmotion.transform.rotation = Quaternion.Euler(-45,0,0);
                return newEmotion;
            }
            
            throw new NullReferenceException("Couldn't get a new emotion");
        }

        private static void OnEmotionAttached(EmotionWorld emotionWorld)
        {
            emotionWorld.ActivateCollider(false);
            emotionWorld.ActivateAnimatorState(true);
        }

        private static void OnEmotionDetached(EmotionWorld emotionWorld)
        {
            emotionWorld.ActivateCollider(true);
            emotionWorld.ActivateAnimatorState(false);
        }
    
        #endregion

        # region Fields

        [SerializeField] private Emotion _emotion;
    
        private SpriteRenderer _spriteRenderer;
    
        private Animator _animator;

        private BoxCollider2D _internalCollider;
    
        [SerializeField] private float _idleAnimationSpeed;

        [System.NonSerialized] public EmotionWorld next;
        
        private static readonly int ChangeStateToChild = Animator.StringToHash("ChangeStateToChild");


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

            EmotionController.OnEmotionAttached += OnEmotionAttached;
            EmotionController.OnEmotionDetached += OnEmotionDetached;

        }

        private void Init(Emotion emotion)
        {
            _emotion = emotion;
            _spriteRenderer.sprite = emotion.GetSprite();
            _animator.runtimeAnimatorController = emotion.GetAnimatorController();
            Instantiate(emotion.GetParticleObject(), transform);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") && !other.CompareTag("Consumable")) return;
            
            Debug.Log("Enter internal collider of emotion controller holder parent");
            var isHandled = other.GetComponentInChildren<EmotionController>().Handle(_emotion);     // any way to create callback ?
        
            if (isHandled)
            {
                this.gameObject.SetActive(false);
                OnDeactivate?.Invoke(this);
            }
        }

        public void ActivateCollider(bool state) => _internalCollider.enabled = state;
        
        private void ActivateAnimatorState(bool state) => _animator.SetBool(ChangeStateToChild, state);

        private void FixedUpdate()
        {
            transform.position += new Vector3(0, _idleAnimationSpeed / 1000, 0);
        }

        # endregion
   
    }
}
