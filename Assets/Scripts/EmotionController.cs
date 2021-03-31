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
    public EmotionColor emotionColor;
    bool isActive = false;

    public Emotion(EmotionColor ec, bool isA)
    {
        emotionColor = ec;
        isActive = isA;
    }

    public EmotionColor EmotionColor { get => emotionColor; set => emotionColor = value; }

    public Sprite GetSprite()
    {
        switch (emotionColor)
        {
            default:
            case EmotionColor.blue:     return      Resources.Load("ball_blue")   as Sprite;
            case EmotionColor.green:    return      Resources.Load("ball_green")  as Sprite;
            case EmotionColor.pink:     return      Resources.Load("ball_pink")   as Sprite;
            case EmotionColor.purple:   return      Resources.Load("ball_purple") as Sprite;
            case EmotionColor.yellow:   return      Resources.Load("ball_yellow") as Sprite;
        }
    }
}

public class EmotionController : MonoBehaviour
{
    public List<Emotion> emotions = new List<Emotion>();
    float stepAngle = 45;
    float globalAngle = -45;
    public GameObject emotionWorld;
    public Vector3 direction;
    
    public void SetEmotionWorld(GameObject ew)
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
            if (emotions.Count < 5)
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
            if (emotions.Count > 0) // prevent IndexOutOfRangeException for empty list
            {
                var emotionToUndraw = RemoveEmotion();
                UndrawEmotion();
                DropEmotion(this.gameObject.transform.position, emotionToUndraw.EmotionColor);
            }

        }

        // show all emotions in console !!! 
        Debug.Log("Emotions count: " + emotions.Count);
        foreach (var emotion in emotions)
        {
            Debug.Log(emotion.EmotionColor.ToString());
        }

        if (emotions.Count == 5)
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
    private IEnumerator fiveSpheres()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Heal");
        for (int i = 0; i < 5; i++)
        {
            var emotionToUndraw = RemoveEmotion();
            // UndrawEmotion();
            Destroy(transform.GetChild(i).gameObject);
            Debug.Log(emotions.Count);
        }
        GetComponentInParent<PlayerHealth>().UpdateHealth(+50);
        GetComponentInParent<PlayerHealth>().healthReduceValue += 0.001f;
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
        Destroy( transform.GetChild(transform.childCount - 1).gameObject ); 
        globalAngle += stepAngle;
    }

    public GameObject SpawnEmotion(Vector3 position, EmotionColor emotionColor)
    {
        var spawnedEmotion = Instantiate(GetObjectBy(emotionColor), position, Quaternion.identity) 
        as GameObject;
        return spawnedEmotion;
    }

    public void SpawnEmotionAsChild(float angle, EmotionColor emotionColor)
    {
        direction = (Quaternion.Euler(0, 0, angle) * Vector3.down).normalized;
        GameObject emotionObject = Instantiate(GetObjectBy(emotionColor), transform.position, Quaternion.identity)
        as GameObject;
        emotionObject.transform.SetParent(this.gameObject.transform, true);
        // emotionObject.transform.position = transform.position + direction * radius;  // replaced into magnet mechanic 
        emotionObject.transform.position = transform.position;
    }

    public void DropEmotion(Vector3 dropPosition, EmotionColor emotionColor)
    {
        Vector3 randomDir = Helper.GetRandomDir();
        GameObject emotionWorld = SpawnEmotion(dropPosition + randomDir, emotionColor);
        // emotionWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 40f, ForceMode2D.Impulse);
    }
}
