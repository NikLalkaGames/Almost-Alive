using System;
using System.Runtime.CompilerServices;
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
        private BoxCollider _internalCollider;
        
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
            _internalCollider = GetComponent<BoxCollider>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Mob") || other.CompareTag("Player"))
            {
                var emotionController = other.GetComponentInChildren<EmotionController>();
                if (emotionController == null) return;
                
                Debug.Log("Emotion has entered into internal collider ");
                emotionController.Handle(this);
                ActivateCollider(false);    
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
