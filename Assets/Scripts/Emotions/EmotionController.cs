using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionController : MonoBehaviour
{
    # region Fields

    // emotions
    protected List<Emotion> _emotions = new List<Emotion>();
    private int _emotionAmount;
    
    // angles for emotions adding
    private float stepAngle = 45;
    protected float globalAngle = -180;
    public Vector3 directionOfAttaching;

    // event
    public static event System.Action onHandle;
    
    # region Properties

    public Vector3 DirectionOfDrop
    {
        get
        {
            if (GetComponentInParent<GhostMovement>() != null)
            {
                return GetComponentInParent<GhostMovement>().LookDirection;
            }
            else
            {
                return Helper.GetRandomDir();
            }
        }
    }

    public List<Emotion> Emotions { get => _emotions; }

    public GameObject EmotionWorld { get; set; }

    # endregion

    # endregion

    # region Internal Methods

    public void SaveEmotionWorld(GameObject ew) => EmotionWorld = ew;

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

    public void Handle(EmotionColor ec)
    {
        if (ec != EmotionColor.none)
        {
            if (!EmotionExists(ec) && _emotions.Count < 5)
            {
                var emotionToDraw = AddEmotion(ec);
                if (emotionToDraw != null)  // prevent null reference exception for existing emotion
                {
                    DrawNewEmotion(emotionToDraw.EmotionColor);
                }
            }
        }
        else
        {
            if (_emotions.Count > 0)         // prevent IndexOutOfRangeException for empty list
            {
                var emotionToUndraw = RemoveEmotion();
                UndrawEmotion();
                DropEmotion(this.gameObject.transform.position, this.DirectionOfDrop, emotionToUndraw.EmotionColor);
            }

        }

        if (onHandle != null)
            onHandle?.Invoke();

        Debug.Log("Emotions count: " + _emotions.Count);

        foreach (var emotion in _emotions)
        {
            Debug.Log(emotion.EmotionColor.ToString());
        }

        if (_emotions.Count == 5)
        {
            if (GetComponentInParent<GhostMovement>() != null)
            {
                StartCoroutine("fiveSpheres");
            }
        }
    }

  

    // fix coroutine bag
    // ONLY FOR PLAYER
    protected IEnumerator fiveSpheres()
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
                Debug.Log(_emotions.Count);
            }
            
            ghostHealth.UpdateHealth(+50);
            ghostHealth.IncreaseHealthReduction();

            globalAngle = -180;
        }
    }

    public bool EmotionExists(EmotionColor ec)
    {
        if (_emotions.Exists(x => x.EmotionColor == ec))
        {
            Debug.Log("This emotion is already exists!");
            return true;
        }
        else
        {
            return false;
        }
    }

    protected Emotion AddEmotion(EmotionColor ec)
    {
        var emotionToAdd = new Emotion(ec);
        _emotions.Add(emotionToAdd);

        // add request to change state to "TransformAboveHead" for emotion gameOjbect in scene
        
        Destroy(EmotionWorld);
        EmotionWorld = null;
        return emotionToAdd;
    }

    protected Emotion RemoveEmotion()
    {
        var emotionToDrop = _emotions[_emotions.Count - 1];
        _emotions.Remove(emotionToDrop);
        return emotionToDrop;
    }


    public void DrawNewEmotion(EmotionColor ec)
    {
        SpawnEmotionAsChild(globalAngle, ec);
        globalAngle -= stepAngle;
    }

    public void UndrawEmotion()
    {
        globalAngle += stepAngle;
        Destroy( transform.GetChild(transform.childCount - 1).gameObject ); 
    }

    public GameObject SpawnEmotion(Vector3 position, EmotionColor emotionColor)
    {
        var spawnedEmotion = Instantiate(GetObjectBy(emotionColor), position, Quaternion.identity) 
        as GameObject;
        return spawnedEmotion;
    }

    public void SpawnEmotionAsChild(float angle, EmotionColor emotionColor)
    {
        directionOfAttaching = (Quaternion.Euler(0, 0, angle) * Vector3.right).normalized;
        GameObject emotionObject = Instantiate(GetObjectBy(emotionColor), transform.position, Quaternion.identity)
        as GameObject;
        emotionObject.transform.SetParent(this.gameObject.transform, true);
        // emotionObject.transform.position = transform.position + direction * radius;  // replaced into magnet mechanic 
        emotionObject.transform.position = transform.position;
    }

    public void DropEmotion(Vector3 dropPosition, Vector3 direction, EmotionColor emotionColor)
    {
        GameObject emotionWorld = SpawnEmotion(dropPosition + direction, emotionColor);
        // emotionWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 40f, ForceMode2D.Impulse);
    }

    # endregion
}
