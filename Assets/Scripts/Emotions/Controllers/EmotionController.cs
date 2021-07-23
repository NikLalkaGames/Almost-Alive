using System;
using System.Collections;
using System.Collections.Generic;
using Core.CollisionDetecting;
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

        protected List<Transform> _emotionHolders = new List<Transform>(5);

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
        public static event Action OnHandle;
        
        /// <summary>
        /// Configure deactivated emotion callback invoker after emotion detaching    
        /// </summary>
        public static event Action<EmotionWorld> OnEmotionDetached;

        # endregion

        # region Internal Methods

        protected virtual void Awake()
        {
            EmotionColliderDetector.OnEmotionCheck += EmotionExists;
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

        protected void CreateEmotionHolders()
        {
            var angle = -180f;

            for (int i = 0; i < MAX_EMOTIONS_AMOUNT; i++)
            {          
                var direction = (Quaternion.Euler(0, 0, angle) * Vector3.right).normalized;
            
                var emotionHolder = Instantiate(new GameObject(), 
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

        public bool EmotionExists(Collider2D other) => 
            _emotions.Exists(e => e.Color == other.GetComponent<EmotionWorld>().Emotion.Color);
        

        # region Emotions data manipulation

        protected Transform AddEmotion(Emotion emotion)
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
    
        public Transform AttachEmotion(Emotion emotion)
        {
            var emotionToAttach = EmotionWorld.TakeFromPoolAndPlace(transform.position, emotion).transform;

            emotionToAttach.SetParent(_emotionHolders[LastEmotion], true);

            emotionToAttach.GetComponent<Collider2D>().enabled = false;         // may be replace on events ?

            return emotionToAttach;
        }

        public Transform DetachEmotion()
        {
            var emotionToDeactivate = _emotionHolders[LastEmotion].GetChild(0);

            emotionToDeactivate.gameObject.SetActive(false);
        
            emotionToDeactivate.SetParent(_emotionObjectPool, true);          // return to pool or can fully unparent
            
            emotionToDeactivate.GetComponent<Collider2D>().enabled = true;
            
            OnEmotionDetached?.Invoke(emotionToDeactivate.GetComponent<EmotionWorld>());

            return emotionToDeactivate;
        }

        protected IEnumerator LerpTo(Transform emotionToAttach, Transform destTransform)
        {
            while (!Helpers.Reached(emotionToAttach.position, destTransform.position))
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

        protected IEnumerator WaitForLerp(Transform emotionToAttach, Transform destTransform)
        {
            yield return StartCoroutine( LerpTo(emotionToAttach, destTransform) );
        }

        # endregion

        # endregion
    }
}
