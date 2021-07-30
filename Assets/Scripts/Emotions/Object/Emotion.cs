using System;
using Emotions.Controllers;
using Emotions.Models;
using UnityEngine;

namespace Emotions.Object
{
    public class Emotion : MonoBehaviour
    {
        # region External usage

        public void OnEmotionAttached()
        {
            this.ActivateCollider(false);
            this.ActivateAnimatorState(true);
        }

        public void OnEmotionDetached()
        {
            this.ActivateCollider(true);
            this.ActivateAnimatorState(false);
        }
    
        #endregion

        # region Fields

        // Emotion data
        [SerializeField] private EmotionColor color;
        public string category;

        // collision 
        private BoxCollider2D _internalCollider;
        
        // animator behaviour
        [SerializeField] private float idleAnimationSpeed;
        private Animator _animator;
        private static readonly int ChangeStateToChild = Animator.StringToHash("ChangeStateToChild");
        
        // Color property
        public EmotionColor Color => color;

        #endregion

        #region Methods

        private void Awake()
        {
            _internalCollider = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
            
            //EmotionController.OnEmotionAttached += OnEmotionAttached;
            //EmotionController.OnEmotionDetached += OnEmotionDetached;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") && !other.CompareTag("Consumable")) return;
            
            Debug.Log("Enter internal collider of emotion controller holder parent");
            var isHandled = other.GetComponentInChildren<EmotionController>().Handle(this);     // any way to create callback ?
        
            if (isHandled)
            {
                // this.gameObject.SetActive(false);
                // OnDeactivate?.Invoke(this);
            }
        }

        public void ActivateCollider(bool state) => _internalCollider.enabled = state;
        
        private void ActivateAnimatorState(bool state) => _animator.SetBool(ChangeStateToChild, state);

        private void FixedUpdate()
        {
            transform.position += new Vector3(0, idleAnimationSpeed / 1000, 0);
        }

        # endregion
   
    }
}
