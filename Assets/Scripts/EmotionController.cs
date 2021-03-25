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
    none
}

public class Emotion
{
    EmotionColor emotionColor;
    bool isActive = false;

    public Emotion(EmotionColor ec, bool isA)
    {
        emotionColor = ec;
        isActive = isA;
    }

    public EmotionColor EmotionColor { get => emotionColor; set => emotionColor = value; }
}

public class EmotionController : MonoBehaviour
{
    private PlayerController player;
    private List<Emotion> emotions = new List<Emotion>();

    float stepAngle = 45;
    float globalAngle = -45;

    public GameObject emotionWorld;
    public void SetEmotionWorld(GameObject ew)
    {
        emotionWorld = ew;
    }

    public Vector3 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.staticController;
    }
    
    public GameObject GetEmotionObjectByColor(EmotionColor emotionColor)
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
            if (emotions.Count < 5)
            {
                var emotionToDraw = AddEmotion(ec);
                if (emotionToDraw != null)
                {
                    DrawNewEmotion(emotionToDraw.EmotionColor);
                }
            }
            
        }
        else
        {
            Debug.Log("Drop emotion(none): ");
            if (emotions.Count > 0) //prevent IndexOutOfRangeException for empty list
            {
                var emotionToUndraw = RemoveEmotion();
                UndrawEmotion();
                DropEmotion(this.gameObject.transform.position, emotionToUndraw.EmotionColor);
            }

        }

        foreach (var emotion in emotions)
        {
            Debug.Log(emotion.EmotionColor.ToString());
        }

        if (emotions.Count == 5)
        {
            StartCoroutine("fiveSpheres");
        }
    }

    private IEnumerator fiveSpheres()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Heal");
        for (int i = 1; i < 6; i++)
        {
            var emotionToUndraw = RemoveEmotion();
            Destroy(transform.GetChild(i).gameObject);
            Debug.Log(emotions.Count);
        }
        GetComponent<PlayerHealth>().UpdateHealth(+50);
        GetComponent<PlayerHealth>().healthReduceValue += 0.001f;
        globalAngle = -45;
    }

    private Emotion AddEmotion(EmotionColor ec)
    {
        var emotionToAdd = new Emotion(ec, true);
        
        if ( emotions.Exists(x => x.EmotionColor == ec) )
        {
            Debug.Log("This emotion is already exists!");
            return null;
        }
        else
        {
            emotions.Add(emotionToAdd);
            Destroy(emotionWorld);
            emotionWorld = null;
            return emotionToAdd;
        }
    }

    private Emotion RemoveEmotion()
    {
        var emotionToDrop = emotions[emotions.Count - 1];
        emotions.RemoveAt(emotions.Count - 1);
        return emotionToDrop;
    }


    public void DrawNewEmotion(EmotionColor ec)
    {
        globalAngle -= stepAngle;
        SpawnEmotionAsChild(globalAngle, ec);
    }

    public void UndrawEmotion()
    {
        Debug.Log( gameObject.transform.childCount );
        Destroy( transform.GetChild ( gameObject.transform.childCount - 1 ).gameObject  ); 
        globalAngle += stepAngle;
    }

    public GameObject SpawnEmotion(Vector3 position, EmotionColor emotionColor)
    {
        var spawnedEmotion = Instantiate(GetEmotionObjectByColor(emotionColor), position, Quaternion.identity) 
        as GameObject;
        return spawnedEmotion;
    }

    public void SpawnEmotionAsChild(float angle, EmotionColor emotionColor)
    {
        direction = (Quaternion.Euler(0, 0, angle) * Vector3.down).normalized;
        GameObject emotionObject = Instantiate(GetEmotionObjectByColor(emotionColor), transform.position, Quaternion.identity)
        as GameObject;
        emotionObject.transform.SetParent(this.gameObject.transform, false);
        //emotionObject.transform.position = transform.position + direction * radius;
        emotionObject.transform.position = transform.position;
    }

    public void DropEmotion(Vector3 dropPosition, EmotionColor emotionColor)
    {
        Vector3 randomDir = GetRandomDir();
        GameObject emotionWorld = SpawnEmotion(dropPosition + randomDir, emotionColor);
        // emotionWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 40f, ForceMode2D.Impulse);
    }

    public static Vector3 GetRandomDir()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }
}
