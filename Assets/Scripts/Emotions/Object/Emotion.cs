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
            ActivateCollider(false);
            ActivateAnimatorState(true);
        }

        public void OnEmotionDetached()
        {
            ActivateCollider(true);
            ActivateAnimatorState(false);
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
        
        public void ActivateCollider(bool state) => _internalCollider.enabled = state;
        
        private void ActivateAnimatorState(bool state) => _animator.SetBool(ChangeStateToChild, state);

        private void FixedUpdate()
        {
            transform.position += new Vector3(0, idleAnimationSpeed / 1000, 0);
        }

        # endregion
   
    }
}
