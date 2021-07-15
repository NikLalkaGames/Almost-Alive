using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmotionController : MonoBehaviour
{
    #region Fields
    
    protected List<Emotion> _emotions = new List<Emotion>(5);
    
    [SerializeField] protected float _dropRadius;

    protected List<Transform> _emotionHolders = new List<Transform>(5);

    public EmotionObjectPool _emotionPool;

    
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
    public static event Action<EmotionWorld> OnEmotionDroped;

    # endregion

    # region Internal Methods

    protected virtual void Start() => CreateEmotionHolders();

    protected void CreateEmotionHolders()
    {
        float angle = -180f;

        for (int i = 0; i < MAX_EMOTIONS_AMOUNT; i++)
        {          
            var direction = (Quaternion.Euler(0, 0, angle) * Vector3.right).normalized;
            
            GameObject emotionHolder = Instantiate(new GameObject(), transform.position, Quaternion.identity)
            as GameObject;

            emotionHolder.transform.SetParent(this.transform, true);
            emotionHolder.transform.position = transform.position + direction;

            _emotionHolders.Add(emotionHolder.transform);

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
        return AttachEmotion(emotion);
    }

    protected Emotion RemoveEmotion()
    {
        DropEmotion();
        
        var droppedEmotion = _emotions[LastEmotion];
        _emotions.Remove(droppedEmotion);

        return droppedEmotion;
    }

    # endregion


    # region Emotion Transform methods
    
    public Transform AttachEmotion(Emotion emotion)
    {
        var emotionToAttach = EmotionWorld.TakeFromPoolAndPlace(transform.position, emotion).transform;

        emotionToAttach.SetParent(_emotionHolders[LastEmotion], true);

        return emotionToAttach;
    }

    public void DropEmotion()
    {
        var emotionToDeactivate = _emotionHolders[LastEmotion].GetChild(0);

        emotionToDeactivate.SetParent(null, true);

        emotionToDeactivate.gameObject.SetActive(false);
        
        OnEmotionDroped?.Invoke(emotionToDeactivate.GetComponent<EmotionWorld>());
        
        // spawn new emotion object from object pool       
        EmotionWorld.TakeFromPoolAndPlace(transform.position + DirectionOfDrop * _dropRadius, _emotions[LastEmotion]);
    }

    protected IEnumerator LerpTo(Transform emotionToAttach, Transform destTransform)
    {
        while (!Helper.Reached(emotionToAttach.position, destTransform.position))
        {
            yield return new WaitForEndOfFrame();
            
            if (emotionToAttach.parent == null)
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
