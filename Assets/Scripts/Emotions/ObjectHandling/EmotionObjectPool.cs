using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionObjectPool : MonoBehaviour
{
    public static EmotionObjectPool Instance { get; private set; }
    
    public EmotionWorld EmotionWorldPrefab;

    private List<EmotionWorld> _worldEmotions = new List<EmotionWorld>();

    private const int INITIAL_POOL_SIZE = 20;

    private const int MAX_POOL_SIZE = 40;

    private EmotionWorld firstAvailable;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        EmotionController.OnEmotionDetached += ConfigureDeactivatedObject;
        EmotionWorld.OnDeactivate += ConfigureDeactivatedObject;
    }

    private void Start()
    {
        if (EmotionWorldPrefab == null)
        {
            Debug.LogError("Need a reference to the bullet prefab");
        }

        for (int i = 0; i < INITIAL_POOL_SIZE; i++)
        {
            GenerateEmotion();
        }

        firstAvailable = _worldEmotions[0];

        for (int i = 0; i < _worldEmotions.Count - 1; i++)
        {
            _worldEmotions[i].next = _worldEmotions[i + 1];
        }

        _worldEmotions[_worldEmotions.Count - 1].next = null;
    }

    private void GenerateEmotion()
    {
        var newEmotion = Instantiate(EmotionWorldPrefab, transform);

        newEmotion.gameObject.SetActive(false);

        _worldEmotions.Add(newEmotion);
    }

    public void ConfigureDeactivatedObject(EmotionWorld deactivatedObj)
    {
        deactivatedObj.next = firstAvailable;

        firstAvailable = deactivatedObj;
    }


    public EmotionWorld GetEmotion()
    {
        //Instead of searching a list to find an inactive object, we simply get the firstAvilable object
        if (firstAvailable == null)
        {
            //We are out of bullets so we have to instantiate another bullet (if we can)
            if (_worldEmotions.Count < MAX_POOL_SIZE)
            {
                GenerateEmotion();

                //The new emotion is last in the list so get it
                EmotionWorld lastBullet = _worldEmotions[_worldEmotions.Count - 1];

                //Add it to the linked list by reusing the method we use for deactivated bullets, so it will now be the first bullet in the linked-list
                ConfigureDeactivatedObject(lastBullet);
            }
            else
            {
                throw new System.OverflowException("Emotion pool overflowed");
            }
        }

        //Remove it from the linked-list
        EmotionWorld emotionToActivate = firstAvailable;

        firstAvailable = emotionToActivate.next;

        return emotionToActivate;
    }
}
