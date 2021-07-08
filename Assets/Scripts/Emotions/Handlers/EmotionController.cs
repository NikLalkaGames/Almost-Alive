using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionController : MonoBehaviour
{
    # region Fields

    // emotions
    protected Emotion[] _emotions = new Emotion[5];
    
    // angles for emotions adding
    private float _stepAngle = 45;
    protected float _globalAngle = -180;
    public Vector3 DirectionOfAttaching { get; private set; }

    #endregion

    #region Properties

    public Vector3 DirectionOfDrop
    {
        get
        {
            var ghostMovement = GetComponentInParent<GhostMovement>();
            if (ghostMovement != null)
            {
                return ghostMovement.LookDirection;
            }
            else
            {
                return Helper.GetRandomDir();
            }
        }
    }

    public Emotion[] Emotions => _emotions;

    #endregion

    #region Events

    public static event Action onHandle;
    public static event Action<EmotionColor> OnEmotionAdded;
    public static event Action<EmotionColor> OnEmotionRemoved;

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

    private void Start()
    {
        OnEmotionAdded += DrawNewEmotion;

        OnEmotionRemoved += UndrawEmotion;
        OnEmotionRemoved += DropEmotion;
    }

    private void Update()
    {
        // Drop emotion logic
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (EmotionsCount > 0)         // prevent IndexOutOfRangeException for empty list
            {
                var emotionToUndraw = RemoveEmotion();
            }
        }
    }

    public void Handle(EmotionColor emotionColor)
    {
        if (!EmotionExists(emotionColor))
        {
            var emotionToDraw = AddEmotion(emotionColor);
        }

        if (onHandle != null)
            onHandle?.Invoke();

        Debug.Log("Emotions Count: " + EmotionsCount);

        if (EmotionsCount == 5)
        {
            if (GetComponentInParent<GhostMovement>() != null)
            {
                StartCoroutine("fiveSpheres");
            }
        }
    }

    // TODO: fix coroutine bag
    private IEnumerator fiveSpheres()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Heal");
        
        var ghostHealth = GetComponentInParent<GhostHealth>();

        if (ghostHealth != null)
        {
            for (int i = 0; i < 5; i++)
            {
                var emotionToUndraw = RemoveEmotion();
                // UndrawEmotion();
                Destroy(transform.GetChild(i).gameObject);
            }
            
            ghostHealth.UpdateHealth(+50);
            ghostHealth.IncreaseHealthReduction();

            _globalAngle = -180;
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
        var emotionIsAdded = AddToArray(emotionToAdd);

        if (emotionIsAdded)
        {
            OnEmotionAdded?.Invoke(emotionToAdd.EmotionColor);
            return emotionToAdd;
        }

        throw new OperationCanceledException("Emotion hasn't added to emotion array");
    }

    protected Emotion RemoveEmotion()
    {
        var emotionToDrop = _emotions[EmotionsCount - 1];
        
        if (emotionToDrop != null)
        {
            _emotions[EmotionsCount - 1] = null;
            OnEmotionRemoved.Invoke(emotionToDrop.EmotionColor);
            return emotionToDrop;
        }

        throw new Exception("Emotion hasn't removed from the emotion array");
    }

    # endregion

    # region Array Methods and Properties

    protected bool AddToArray(Emotion emotionToAdd)
    {
        for (int i = 0; i < _emotions.Length; i++)
        {
            if (_emotions[i] == null)
            {
                _emotions[i] = emotionToAdd;
                return true;
            }
        }

        return false;
    }

    protected int EmotionsCount
    {
        get 
        {
            int count = 0;
        
            for (int i = 0; i < _emotions.Length; i++)
            {
                if (_emotions[i] != null)
                {
                    count++;
                }
            }

            return count;
        }
    }

    # endregion 

    # region Emotion Transform methods

    public void DrawNewEmotion(EmotionColor emotionColor)
    {
        SpawnEmotionAsChild(_globalAngle, emotionColor);
        _globalAngle -= _stepAngle;
    }

    public void UndrawEmotion(EmotionColor emotionColor)
    {
        _globalAngle += _stepAngle;
        Destroy( transform.GetChild(transform.childCount - 1).gameObject );
    }

    public void SpawnEmotionAsChild(float angle, EmotionColor emotionColor)
    {
        DirectionOfAttaching = (Quaternion.Euler(0, 0, angle) * Vector3.right).normalized;
        GameObject emotionObject = Instantiate(GetObjectBy(emotionColor), transform.position, Quaternion.identity)
        as GameObject;
        emotionObject.transform.SetParent(this.gameObject.transform, true);
        // emotionObject.transform.position = transform.position + direction * radius;  // replaced into magnet mechanic 
        emotionObject.transform.position = transform.position;
    }

    public void DropEmotion(EmotionColor emotionColor)
    {
        GameObject emotionWorld = Instantiate(GetObjectBy(emotionColor), transform.position + DirectionOfDrop, Quaternion.identity) 
        as GameObject;
        // emotionWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 40f, ForceMode2D.Impulse);
    }

    # endregion

    # endregion
}
