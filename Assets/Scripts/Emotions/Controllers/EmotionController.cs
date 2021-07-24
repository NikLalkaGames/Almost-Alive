using System;
using System.Collections;
using System.Collections.Generic;
using Core.CollisionDetecting;
using Core.CollisionDetection;
using static Core.Helpers.Helpers;
using Emotions.Models;
using Emotions.ObjectHandling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Emotions.Controllers
{
    public class EmotionController : MonoBehaviour
    {
        #region Fields
    
        protected List<Emotion> _emotions = new List<Emotion>(5);
    
        [SerializeField] protected float dropRadius;

        private List<Transform> _emotionHolders = new List<Transform>(5);

        private Transform _emotionObjectPool;
    
        private const int MAX_EMOTIONS_AMOUNT = 5;
        

        protected int LastEmotion => _emotions.Count - 1;

        private Transform _transform;

        #endregion

        #region Properties

        protected virtual Vector3 DirectionOfDrop { get; set; }

        public List<Emotion> Emotions => _emotions;

        #endregion

        #region Events
        
        /// <summary>
        /// Little humans event for defining colors on handle
        /// </summary>
        public static event Action OnHandle;        // TODO: Debug and understand how it properly work
        
        /// <summary>
        /// Configure deactivated emotion callback invoker after emotion detaching    
        /// </summary>
        public static event Action<EmotionWorld> OnEmotionDetached;

        public static event Action<EmotionWorld> OnEmotionAttached;

        # endregion

        # region Internal Methods

        protected virtual void Awake()
        {
            // EmotionColliderDetector.OnEmotionCheck += EmotionExists;
        }

        protected virtual void Start()
        {
            _transform = transform;
            
            CreateEmotionHolders();

            if (EmotionObjectPool.Instance == null)
            {
                Debug.LogError($"Need emotion object pool gameObject on scene");
            }

            _emotionObjectPool = EmotionObjectPool.Instance.transform;
        }

        private void CreateEmotionHolders()
        {
            var angle = -180f;

            for (var i = 0; i < MAX_EMOTIONS_AMOUNT; i++)
            {          
                var direction = (Quaternion.Euler(0, 0, angle) * Vector3.right).normalized;
            
                var emotionHolder = Instantiate(
                        new GameObject(), 
                        transform.position + direction, 
                        Quaternion.identity, 
                        _transform)
                    .transform;

                _emotionHolders.Add(emotionHolder);

                angle -= 45;
            }
        }
    
        public bool Handle(Emotion emotion)
        {
            if ( !_emotions.Exists(e => e.Color == emotion.Color) )
            {
                var emotionToLerp = AddEmotion(emotion);

                StartCoroutine( LerpTo(emotionToLerp, _emotionHolders[LastEmotion]) );
                
                OnHandle?.Invoke();         // ? can be deleted
                
                Debug.Log("Emotions Count: " + _emotions.Count);

                return true;
            }

            return false;
        }

        public bool EmotionExists(Emotion emotion) => 
            _emotions.Exists(e => e.Color == emotion.Color);
        

        # region Emotions data manipulation

        private Transform AddEmotion(Emotion emotion)
        {
            _emotions.Add(emotion);

            var attachedEmotion = AttachEmotion(emotion);
        
            return attachedEmotion;
        }

        protected Transform RemoveEmotion()
        {
            var unattachedEmotion = DetachEmotion();
        
            _emotions.RemoveAt(LastEmotion);

            return unattachedEmotion;
        }

        protected Transform RemoveAndThrowEmotion()
        {
            EmotionWorld.TakeFromPoolAndPlace(transform.position + DirectionOfDrop * dropRadius, _emotions[LastEmotion]);
        
            var removedEmotion = RemoveEmotion();

            return removedEmotion;
        }

        # endregion


        # region Emotion Transform methods

        private Transform AttachEmotion(Emotion emotion)
        {
            var emotionWorld = EmotionWorld.TakeFromPoolAndPlace(transform.position, emotion);

            //emotionWorld.ActivateCollider(false);
            
            OnEmotionAttached?.Invoke(emotionWorld);
            
            var emotionToAttach = emotionWorld.transform;

            emotionToAttach.SetParent(_emotionHolders[LastEmotion], true);

            return emotionToAttach;
        }

        private Transform DetachEmotion()
        {
            var emotionToDeactivate = _emotionHolders[LastEmotion].GetChild(0);

            emotionToDeactivate.gameObject.SetActive(false);
        
            emotionToDeactivate.SetParent(_emotionObjectPool, true);          // return to pool or can fully unparent

            OnEmotionDetached?.Invoke(emotionToDeactivate.GetComponent<EmotionWorld>());

            return emotionToDeactivate;
        }

        private IEnumerator LerpTo(Transform emotionToAttach, Transform destTransform)
        {
            while (!Reached(emotionToAttach.position, destTransform.position))
            {
                yield return new WaitForEndOfFrame();
            
                if (emotionToAttach.parent == _emotionObjectPool)     // can be null if want to fully unparent
                {
                    yield break;
                }

                emotionToAttach.position = Vector2.Lerp(emotionToAttach.position, destTransform.position, Time.deltaTime * 1.5f);   //transform from player position to 
            }

            Debug.Log("Lerp Finished");
        }

        public static IEnumerator MagnetTo(Transform magnetFrom, Transform magnetTo, float colliderRadius)
        {
            while (!Reached(magnetFrom.position, magnetTo.position))
            {
                yield return new WaitForEndOfFrame();

                var magnetFromPosition = (Vector2) magnetFrom.position;
                var toPosition = magnetTo.position;
                var pickUpSpeed =  colliderRadius - Vector2.Distance(magnetFromPosition, toPosition);
                
                magnetFromPosition = Vector2.MoveTowards(magnetFromPosition, toPosition, pickUpSpeed * Time.deltaTime);
                magnetFrom.position = magnetFromPosition;
            }
        }

        protected IEnumerator WaitForLerp(Transform emotionToAttach, Transform destTransform)
        {
            yield return StartCoroutine( LerpTo(emotionToAttach, destTransform) );
        }

        # endregion

        # endregion
    }
}
