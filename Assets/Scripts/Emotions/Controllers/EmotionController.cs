using System;
using System.Collections;
using System.Collections.Generic;
using Core.Helpers;
using Emotions.Models;
using Emotions.Object;
using ObjectPooling;
using UnityEngine;
using Emotion = Emotions.Object.Emotion;

namespace Emotions.Controllers
{
    public class EmotionController : MonoBehaviour
    {
        #region Fields

        protected List<Emotion> _emotions = new List<Emotion>();    // List<EmotionColor>

        [SerializeField] protected float dropRadius;

        private List<Transform> _emotionHolders = new List<Transform>(5);

        protected PoolManager _poolManager;
    
        private const int MAX_EMOTIONS_AMOUNT = 5;
        

        protected int LastEmotion => _emotions.Count - 1;

        private Transform _transform;

        #endregion

        #region Properties

        protected virtual Vector3 DirectionOfDrop { get; }

        public List<Emotion> Emotions => _emotions;

        #endregion

        #region Events

        /// <summary>
        /// Little humans event for defining colors on handle
        /// </summary>
        protected event Action OnHandle;

        # endregion

        # region Internal Methods

        protected virtual void Start()
        {
            _transform = transform;
            CreateEmotionHolders();

            if (PoolManager.Instance == null)
            {
                Debug.LogError($"Need emotion object pool gameObject on scene");
            }
            _poolManager = PoolManager.Instance;
        }

        //TODO: create new way of positioning emotions in 3d space
        private void CreateEmotionHolders()
        {
            var angle = -180f;

            for (var i = 0; i < MAX_EMOTIONS_AMOUNT; i++)
            {          
                var direction = (Quaternion.Euler(-45, 0, angle) * Vector3.right).normalized;
            
                var emotionHolder = Instantiate(
                    new GameObject(), 
                    transform.position + direction * 0.8f, 
                    Quaternion.identity, 
                    _transform)
                    .transform;
                
                _emotionHolders.Add(emotionHolder);

                //var emotionHolderTransform = emotionHolder.transform;
                //emotionHolderTransform.position = Quaternion.Euler(-45, 0, 0) * emotionHolder.transform.position;

                angle -= 45;
            }
        }

        public bool Handle(Emotion comingEmotion)
        {
            if (_emotions.Exists(e => e.Color == comingEmotion.Color)) return false;
            
            var emotionToLerp = AddEmotion(comingEmotion);

            StartCoroutine( LerpTo(emotionToLerp.transform, _emotionHolders[LastEmotion]) );
                
            OnHandle?.Invoke();         // ? can be deleted
                
            Debug.Log("Emotions Count: " + _emotions.Count);

            return true;
        }

        public bool EmotionExists(Emotion emotion) => 
            _emotions.Exists(e => e.Color == emotion.Color);
        

        # region Emotions data manipulation

        private Emotion AddEmotion(Emotion emotion)
        {
            _emotions.Add(emotion);

            var attachedEmotion = AttachEmotion(emotion);
        
            return attachedEmotion;
        }

        private void RemoveEmotion()
        {
            _emotions.RemoveAt(LastEmotion);
        }

        protected void ReturnEmotion()
        {
            var detachedEmotion = DetachEmotion();
            
            _poolManager.ReturnToPool(detachedEmotion.category, detachedEmotion.gameObject);
                
            RemoveEmotion();
        }

        protected void ThrowEmotion()
        {
            var emotionToThrow = DetachEmotion();

            RemoveEmotion();
            
            emotionToThrow.ActivateCollider(false);     // turn off emotion collider detector magnet behaviour

            var emotionToThrowTransform = emotionToThrow.transform;
            
            StartCoroutine( WaitCoroutine( LerpTo(
                emotionToThrowTransform, 
                emotionToThrowTransform.position + DirectionOfDrop, 
                emotionToThrow) ) );
        }

        # endregion


        # region Emotion Transform methods

        private Emotion AttachEmotion(Emotion emotion)
        {
            emotion.transform.SetParent(_emotionHolders[LastEmotion], true);

            emotion.OnEmotionAttached();

            return emotion;
        }

        private Emotion DetachEmotion()
        {
            var emotionToDetach = _emotions[LastEmotion];
            
            emotionToDetach.transform.SetParent(_poolManager.transform, true);          // emotion returns to pool

            return emotionToDetach;
        }

        private IEnumerator LerpTo(Transform emotionToAttach, Transform destTransform)
        {
            while (!Helpers.Reached(emotionToAttach.position, destTransform.position))
            {
                yield return new WaitForEndOfFrame();
            
                if (emotionToAttach.parent == _poolManager.transform)     // can be null if want to fully unparented
                {
                    yield break;
                }

                emotionToAttach.position = Vector3.Lerp(emotionToAttach.position, destTransform.position, Time.deltaTime * 1.5f);   //transform from player position to 
            }

            Debug.Log("Lerp Finished");
        }

        private static IEnumerator LerpTo(Transform emotionToDrop, Vector3 destPosition, Emotion emotion)
        {
            while (!Helpers.Reached(emotionToDrop.position, destPosition))
            {
                yield return new WaitForEndOfFrame();

                emotionToDrop.position = Vector3.Lerp(emotionToDrop.position, destPosition, Time.deltaTime * 1.5f);
                
                Debug.Log("Lerp Out");
            }
            
            emotion.ActivateCollider(true);
        }
        
        public static void MagnetStep(Transform magnetFrom, Transform magnetTo, float colliderRadius)
        {
            var magnetFromPosition = magnetFrom.position;
            var toPosition = magnetTo.position;
            var pickUpSpeed =  colliderRadius - Vector3.Distance(magnetFromPosition, toPosition);
            
            magnetFromPosition = Vector3.MoveTowards(magnetFromPosition, toPosition, 1.2f * pickUpSpeed * Time.deltaTime);
            magnetFrom.position = magnetFromPosition;
        }

        private IEnumerator WaitCoroutine(IEnumerator coroutine)
        {
            yield return StartCoroutine( coroutine );
        }

        # endregion

        # endregion
    }
}
