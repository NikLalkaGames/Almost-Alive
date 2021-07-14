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

    protected int LastEmotion => _emotions.Count - 1;

    #endregion

    #region Properties

    protected virtual Vector3 DirectionOfDrop { get; set; }

    public List<Emotion> Emotions => _emotions;

    #endregion

    #region Events

    public static event Action OnHandle;
    public static event Action<EmotionColor> OnEmotionAdded;
    public static event Action<EmotionColor> OnEmotionRemoved;
    public static event Action<Vector3> OnEmotionDraw;

    # endregion

    # region Internal Methods

    public GameObject GetObjectBy(EmotionColor emotionColor)
    {
        switch (emotionColor)
        {
            default:
            case EmotionColor.blue: return Resources.Load("ball_blue") as GameObject;
            case EmotionColor.green: return Resources.Load("ball_green") as GameObject;
            case EmotionColor.pink: return Resources.Load("ball_pink") as GameObject;
            case EmotionColor.purple: return Resources.Load("ball_purple") as GameObject;
            case EmotionColor.yellow: return Resources.Load("ball_yellow") as GameObject;
        }
    }

    protected virtual void Start() => CreateEmotionHolders();

    public void Handle(EmotionColor emotionColor)
    {
        if (!_emotions.Exists(x => x.Color == emotionColor))
        {
            var emotionToLerp = AddEmotion(emotionColor);

            StartCoroutine( WaitForLerp(emotionToLerp, _emotionHolders[LastEmotion]) );
        }

        OnHandle?.Invoke();

        Debug.Log("Emotions Count: " + _emotions.Count);
    }

    protected void CreateEmotionHolders()
    {
        float angle = -180f;

        for (int i = 0; i < 5; i++)
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

    # region Emotions array changes

    public bool EmotionExists(EmotionColor emotionColor)
    {
        foreach (var emotion in _emotions)
        {
            if (emotion != null && emotion.Color == emotionColor)
            {
                Debug.Log("This emotion is already exists!");
                return true;
            }
        }

        return false;
    }

    protected Transform AddEmotion(EmotionColor emotionColor)
    {
        var emotionToAdd = new Emotion(emotionColor);
        _emotions.Add(emotionToAdd);

        var attachedEmotionTransform = AttachEmotion(emotionColor);
        return attachedEmotionTransform;
    }

    protected Emotion RemoveEmotion()
    {
        var emotionToDrop = _emotions[LastEmotion];
        _emotions.Remove(emotionToDrop);
        
        return emotionToDrop;
    }

    # endregion


    # region Emotion Transform methods
    
    public Transform AttachEmotion(EmotionColor emotionColor)
    {
        var emotionToAttach = Instantiate(GetObjectBy(emotionColor), transform.position, Quaternion.identity).transform;

        emotionToAttach.SetParent(_emotionHolders[LastEmotion], true);

        return emotionToAttach;
    }

    public void DropEmotion()
    {
        var emotionColor = _emotions[LastEmotion].Color;

        _ = Instantiate(GetObjectBy(emotionColor), transform.position + DirectionOfDrop * _dropRadius, Quaternion.identity)
        as GameObject;

        Destroy(_emotionHolders[LastEmotion].GetChild(0).gameObject);

        // emotionWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 40f, ForceMode2D.Impulse);
    }

    protected IEnumerator LerpTo(Transform emotionToAttach, Transform destTransform)
    {
        while (!Helper.Reached(emotionToAttach.position, destTransform.position))
        {
            yield return new WaitForEndOfFrame();
            
            if (emotionToAttach == null)
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
