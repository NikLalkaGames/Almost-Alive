using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionController : MonoBehaviour
{
    #region Fields

    protected Stack _emotionColors;    
    
    protected List<Emotion> _emotions = new List<Emotion>(5);
    
    [SerializeField] protected float _dropRadius;

    protected List<Transform> _emotionHolders = new List<Transform>(5);

    protected int EmotionsLast => _emotions.Count - 1;

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

    protected void Start()
    {
        CreateEmotionHolders();
    }

    public void Handle(EmotionColor emotionColor)
    {
        if (!_emotions.Exists(x => x.EmotionColor == emotionColor))
        {
            AddEmotion(emotionColor);

            var emotionToLerp = AttachEmotion(emotionColor);

            StartCoroutine( WaitForLerp(emotionToLerp, _emotionHolders[EmotionsLast]) );
        }

        if (OnHandle != null)
            OnHandle?.Invoke();

        Debug.Log("Emotions Count: " + _emotions.Count);
    }

    // TODO: fix coroutine bag
    protected void FiveSpheres()
    {
        Debug.Log("5 sphere heal");
        
        var ghostHealth = GetComponentInParent<GhostHealth>();

        if (ghostHealth != null)
        {
            for (int i = 0; i < _emotions.Capacity; i++)
            {
                RemoveEmotion();
                // UndrawEmotion();
                Destroy(_emotionHolders[i].GetChild(0).gameObject);
            }
            
            ghostHealth.UpdateHealth(+50);
            ghostHealth.IncreaseHealthReduction();
        }
    }

    protected void CreateEmotionHolders()
    {
        float angle = -180f;

        for (int i = 0; i < 5; i++)
        {          
            GameObject emotionHolder = Instantiate(new GameObject(), transform.position, Quaternion.identity)
            as GameObject;

            var direction = (Quaternion.Euler(0, 0, angle) * Vector3.right).normalized;

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
            if (emotion != null && emotion.EmotionColor == emotionColor)
            {
                Debug.Log("This emotion is already exists!");
                return true;
            }
        }

        return false;
    }

    protected Emotion AddEmotion(EmotionColor emotionColor)
    {
        var emotionToAdd = new Emotion(emotionColor);
        _emotions.Add(emotionToAdd);
        return emotionToAdd;
    }

    protected Emotion RemoveEmotion()
    {
        var emotionToDrop = _emotions[EmotionsLast];
        _emotions.Remove(emotionToDrop);
        return emotionToDrop;
    }

    # endregion


    # region Emotion Transform methods
    
    public Transform AttachEmotion(EmotionColor emotionColor)
    {
        GameObject emotionToAttach = Instantiate(GetObjectBy(emotionColor), transform.position, Quaternion.identity)
        as GameObject;

        emotionToAttach.transform.SetParent(_emotionHolders[EmotionsLast], true);

        return emotionToAttach.transform;
    }

    protected IEnumerator LerpTo(Transform emotionToAttach, Transform destTransform)
    {
        while (!Helper.Reached(emotionToAttach.position, destTransform.position))
        {
            yield return new WaitForEndOfFrame();
            
            emotionToAttach.position = Vector2.Lerp(emotionToAttach.position, destTransform.position, Time.deltaTime * 1.5f);   //transform from player position to 
        }   

        Debug.Log("Lerp Finished");
    }

    protected IEnumerator WaitForLerp(Transform emotionToAttach, Transform destTransform)
    {
        yield return StartCoroutine( LerpTo(emotionToAttach, destTransform) );
    }

    public void DropEmotion()
    {
        var emotionColor = _emotions[EmotionsLast].EmotionColor;
        
        _ = Instantiate(GetObjectBy(emotionColor), _emotionHolders[EmotionsLast].position + DirectionOfDrop * _dropRadius, Quaternion.identity) 
        as GameObject;
        
        Destroy(_emotionHolders[EmotionsLast].GetChild(0).gameObject);

        // emotionWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 40f, ForceMode2D.Impulse);
    }

    # endregion

    # endregion
}
