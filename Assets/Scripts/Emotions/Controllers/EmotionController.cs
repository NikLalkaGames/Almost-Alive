using System;
using System.Collections;
using System.Collections.Generic;
using Core.CollisionDetecting;
using Core.CollisionDetection;
using Core.Helpers;
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

        private Transform _emotionObjectPoolLocation;
    
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

            _emotionObjectPoolLocation = EmotionObjectPool.Instance.transform;
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
            var detachedEmotion = DetachEmotion();
        
            _emotions.RemoveAt(LastEmotion);

            return detachedEmotion;
        }

        protected Transform RemoveAndThrowEmotion()
        {
            var emotionToThrow = EmotionWorld.TakeFromPoolAndPlace(_emotionHolders[LastEmotion].position, _emotions[LastEmotion]);

            var emotionThrowTransform = emotionToThrow.transform;
                
            RemoveEmotion();        // return removed emotion to pool and remove emotion data from collection
            
            emotionToThrow.ActivateCollider(false);     // turn off emotion collider detector magnet behaviour
            
            StartCoroutine( WaitCoroutine( LerpTo(emotionThrowTransform, emotionThrowTransform.position + DirectionOfDrop, emotionToThrow) ) );

            return emotionThrowTransform;  // return emotion for drop coroutine
        }

        # endregion


        # region Emotion Transform methods

        private Transform AttachEmotion(Emotion emotion)
        {
            var emotionWorld = EmotionWorld.TakeFromPoolAndPlace(transform.position, emotion);
            
            var emotionToAttach = emotionWorld.transform;

            emotionToAttach.SetParent(_emotionHolders[LastEmotion], true);

            OnEmotionAttached?.Invoke(emotionWorld);

            return emotionToAttach;
        }

        private Transform DetachEmotion()
        {
            var emotionToDeactivate = _emotionHolders[LastEmotion].GetChild(0);

            emotionToDeactivate.gameObject.SetActive(false);
        
            emotionToDeactivate.SetParent(_emotionObjectPoolLocation, true);          // emotion return to pool or can fully be unparented

            OnEmotionDetached?.Invoke(emotionToDeactivate.GetComponent<EmotionWorld>());

            return emotionToDeactivate;     // pooled emotion (stored in emotion object pool)
        }

        private IEnumerator LerpTo(Transform emotionToAttach, Transform destTransform)
        {
            while (!Helpers.Reached(emotionToAttach.position, destTransform.position))
            {
                yield return new WaitForEndOfFrame();
            
                if (emotionToAttach.parent == _emotionObjectPoolLocation)     // can be null if want to fully unparented
                {
                    yield break;
                }

                emotionToAttach.position = Vector2.Lerp(emotionToAttach.position, destTransform.position, Time.deltaTime * 1.5f);   //transform from player position to 
            }

            Debug.Log("Lerp Finished");
        }

        private static IEnumerator LerpTo(Transform emotionToDrop, Vector2 destPosition, EmotionWorld emotionWorld)
        {
            while (!Helpers.Reached(emotionToDrop.position, destPosition))
            {
                yield return new WaitForEndOfFrame();

                emotionToDrop.position = Vector2.Lerp(emotionToDrop.position, destPosition, Time.deltaTime * 1.5f);
                
                Debug.Log("Lerp Out");
            }
            
            emotionWorld.ActivateCollider(true);
        }
        
        public static void MagnetStep(Transform magnetFrom, Transform magnetTo, float colliderRadius)
        {
            var magnetFromPosition = (Vector2) magnetFrom.position;
            var toPosition = magnetTo.position;
            var pickUpSpeed =  colliderRadius - Vector2.Distance(magnetFromPosition, toPosition);
            
            magnetFromPosition = Vector2.MoveTowards(magnetFromPosition, toPosition, pickUpSpeed * Time.deltaTime);
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
