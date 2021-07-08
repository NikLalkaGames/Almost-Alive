using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionController : MonoBehaviour
{
    # region Fields
    
    protected List<Emotion> _emotions = new List<Emotion>(5);
    
    [SerializeField] protected float _dropRadius;

    protected List<Transform> _emotionHolders = new List<Transform>(5);

    protected int EmotionsLast => _emotions.Count - 1;

    #endregion

    #region Properties

    protected Vector3 DirectionOfDrop => GetComponentInParent<GhostMovement>().LookDirection;

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

    private void Update()
    {
        // Drop emotion logic
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (_emotions.Count > 0)         // prevent IndexOutOfRangeException for empty list
            {
                UnattachEmotion();
                var emotionToDrop = RemoveEmotion();
                DropEmotion(emotionToDrop.EmotionColor);
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_emotions.Count == 5)
            {
                FiveSpheres();
            }
        }
    }

    public void Handle(EmotionColor emotionColor)
    {
        if (!_emotions.Exists(x => x.EmotionColor == emotionColor))
        {
            AddEmotion(emotionColor);

            var emotionToLerp = AttachEmotion(emotionColor);
            
            StartCoroutine( LerpTo(emotionToLerp, _emotionHolders[EmotionsLast]) );
        }

        if (OnHandle != null)
            OnHandle?.Invoke();

        Debug.Log("Emotions Count: " + _emotions.Count);
    }

    // TODO: fix coroutine bag
    private void FiveSpheres()
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

    private void CreateEmotionHolders()
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
        GameObject emotionObject = Instantiate(GetObjectBy(emotionColor), transform.position, Quaternion.identity)
        as GameObject;

        emotionObject.transform.SetParent(_emotionHolders[EmotionsLast], true);
        //emotionObject.transform.position = _emotionHolders[EmotionsLast].position;


        return emotionObject.transform;
    }

    public void UnattachEmotion()
    {
        var emotionToDrop = _emotionHolders[EmotionsLast].GetChild(0);
        _emotionHolders[EmotionsLast].GetChild(0).parent = null;
        emotionToDrop.transform.position = transform.position + DirectionOfDrop * _dropRadius;
    }

    private IEnumerator LerpTo(Transform emotionToAttach, Transform destTransform)
    {
        while (!Helper.Reached(emotionToAttach.position, destTransform.position))
        {
            yield return new WaitForEndOfFrame();
            emotionToAttach.position = Vector2.Lerp(emotionToAttach.position, destTransform.position, Time.deltaTime * 1.5f);   //transform from player position to emotionPos
        }
    }

    public void DropEmotion(EmotionColor emotionColor)
    {
        _ = Instantiate(GetObjectBy(emotionColor), transform.position + DirectionOfDrop * _dropRadius, Quaternion.identity) 
        as GameObject;
        // emotionWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 40f, ForceMode2D.Impulse);
    }

    # endregion

    # endregion
}
