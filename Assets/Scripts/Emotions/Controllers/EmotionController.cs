using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionController : MonoBehaviour
{
    #region Fields
    
    protected List<Emotion> _emotions = new List<Emotion>(5);
    
    [SerializeField] protected float _dropRadius;

    protected List<Transform> _emotionHolders = new List<Transform>(5);

    [SerializeField] private Transform _emotionObjectPool;
    
    private const int MAX_EMOTIONS_AMOUNT = 5;

    protected int LastEmotion => _emotions.Count - 1;

    #endregion

    #region Properties

    protected virtual Vector3 DirectionOfDrop { get; set; }

    public List<Emotion> Emotions => _emotions;

    #endregion

    #region Events

    public static event Action OnHandle;
    public static event Action OnEmotionAttached;
    public static event Action<EmotionWorld> OnEmotionDetached;

    # endregion

    # region Internal Methods

    protected virtual void Start() => CreateEmotionHolders();

    protected void CreateEmotionHolders()
    {
        float angle = -180f;

        for (int i = 0; i < MAX_EMOTIONS_AMOUNT; i++)
        {          
            var direction = (Quaternion.Euler(0, 0, angle) * Vector3.right).normalized;
            
            var emotionHolder = Instantiate(new GameObject(), 
                                            transform.position + direction, 
                                            Quaternion.identity, 
                                            transform)
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
            
            OnHandle?.Invoke();
            
            Debug.Log("Emotions Count: " + _emotions.Count);

            return true;
        }

        return false;
    }

    # region Emotions array changes

    protected Transform AddEmotion(Emotion emotion)
    {
        _emotions.Add(emotion);

        var attachedEmotion = AttachEmotion(emotion);
        
        return attachedEmotion;
    }

    protected Transform RemoveEmotion()
    {
        var unattachedEmotion = UnattachEmotion();
        
        _emotions.RemoveAt(LastEmotion);

        return unattachedEmotion;
    }

    protected Transform RemoveAndThrowEmotion()
    {
        EmotionWorld.TakeFromPoolAndPlace(transform.position + DirectionOfDrop * _dropRadius, _emotions[LastEmotion]);
        
        var removedEmotion = RemoveEmotion();

        return removedEmotion;
    }

    # endregion


    # region Emotion Transform methods
    
    public Transform AttachEmotion(Emotion emotion)
    {
        var emotionToAttach = EmotionWorld.TakeFromPoolAndPlace(transform.position, emotion).transform;

        emotionToAttach.SetParent(_emotionHolders[LastEmotion], true);

        return emotionToAttach;
    }

    public Transform UnattachEmotion()
    {
        var emotionToDeactivate = _emotionHolders[LastEmotion].GetChild(0);

        emotionToDeactivate.gameObject.SetActive(false);
        
        emotionToDeactivate.SetParent(_emotionObjectPool, true);          // return to pool or can fully unparent

        // send notification to object pool to configure new disabled emotion
        OnEmotionDetached?.Invoke(emotionToDeactivate.GetComponent<EmotionWorld>());

        return emotionToDeactivate;
    }

    protected IEnumerator LerpTo(Transform emotionToAttach, Transform destTransform)
    {
        while (!Helper.Reached(emotionToAttach.position, destTransform.position))
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
