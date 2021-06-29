using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EmotionColor
{
    blue,
    green,
    pink,
    purple,
    yellow,
    white,
    none
}

public class Emotion
{
    private EmotionColor emotionColor;
    private bool isActive = false;

    public Emotion(EmotionColor ec, bool isA)
    {
        emotionColor = ec;
        isActive = isA;
    }

    public EmotionColor EmotionColor { get => emotionColor; set => emotionColor = value; }
}

public class EmotionController : MonoBehaviour
{
    protected List<Emotion> emotions = new List<Emotion>();
    
    float stepAngle = 45;

    protected float globalAngle = -180;

    public UnityEvent onHandle;
    
    public GameObject emotionWorld;
    
    public Vector3 directionOfAttaching;

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

    public List<Emotion> Emotions { get => emotions; }

    public void SaveEmotionWorld(GameObject ew)
    {
        emotionWorld = ew;
    }

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
            if (!EmotionExists(ec) && Emotions.Count < 5)
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
            Debug.Log("Drop last emotion: ");
            if (Emotions.Count > 0) // prevent IndexOutOfRangeException for empty list
            {
                var emotionToUndraw = RemoveEmotion();
                UndrawEmotion();
                DropEmotion(this.gameObject.transform.position, this.DirectionOfDrop, emotionToUndraw.EmotionColor);
            }

        }
        if (onHandle != null)
            onHandle.Invoke();      // event calling

        // show all emotions in console !!! 
        Debug.Log("Emotions count: " + Emotions.Count);
        foreach (var emotion in Emotions)
        {
            Debug.Log(emotion.EmotionColor.ToString());
        }

        if (Emotions.Count == 5)
        {
            if (this.transform.parent.CompareTag("Player"))
                StartCoroutine("fiveSpheres");
            else 
            if (this.transform.parent.CompareTag("Consumable"))
                Debug.Log("Consumable has have 5 orbes");
                // need to add functionality for 5 orbs for consumable
            else 
            if (this.transform.parent.CompareTag("Enemy"))
                Debug.Log("Enemy has have 5 orbes");
                // to nothing at while, may be later add some functionality
        }
    }

    // fix coroutine bag
    // ONLY FOR PLAYER
    protected IEnumerator fiveSpheres()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Heal");
        for (int i = 0; i < 5; i++)
        {
            var emotionToUndraw = RemoveEmotion();
            // UndrawEmotion();
            Destroy(transform.GetChild(i).gameObject);
            Debug.Log(Emotions.Count);
        }
        GetComponentInParent<PlayerHealth>().UpdateHealth(+50);
        GetComponentInParent<PlayerHealth>().healthReduceValue += 0.001f;
        globalAngle = -180;
    }

    public bool EmotionExists(EmotionColor ec)
    {
        if (Emotions.Exists(x => x.EmotionColor == ec))
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
        var emotionToAdd = new Emotion(ec, true);
        Emotions.Add(emotionToAdd);
        // add request to change state to "TransformAboveHead" for emotion gameOjbect in scene
        Destroy(emotionWorld);
        emotionWorld = null;
        return emotionToAdd;
    }

    protected Emotion RemoveEmotion()
    {
        var emotionToDrop = Emotions[Emotions.Count - 1];
        Emotions.Remove(emotionToDrop);
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
}
