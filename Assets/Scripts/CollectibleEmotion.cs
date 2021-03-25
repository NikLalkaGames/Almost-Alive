using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleEmotion : MonoBehaviour
{
    EmotionController emotionController;

    public EmotionColor emotionColor;
    public float math;

    private CollectibleEmotion emotion;

    private bool emotionState = true;
    public float amplitude;          //Set in Inspector 
    public float speed;                  //Set in Inspector 
    private float tempVal;
    private Vector3 tempPos;
    private float pickUpSpeed;

    private float distanceToPlayer;
    private bool magnetState = true;
    private Vector3 direction;
    private bool followState = false;
    private bool onPositionState = false;
    private Transform playerT;
    private Vector3 emotionPos;
    public float radius;

    private void Start() 
    {
        if (GetComponentInParent<EmotionController>() != null)
        {
            emotionState = false;
            magnetState = false;

            playerT = GetComponentInParent<PlayerController>().transform;
            direction = GetComponentInParent<EmotionController>().direction;
            
            onPositionState = true;

        }
        emotionController = PlayerController.staticController.transform.GetComponent<EmotionController>();
        tempPos = transform.position;
        tempVal = transform.position.y; 
    }



    private void Update()
    {
        Animotion();
        MagnetToPlayer();
        Animotion2();
    }


    private void Animotion()
    {
        if (emotionState == true)
        {
            tempPos.y = tempVal + amplitude * Mathf.Sin(speed * Time.time);
            transform.position = tempPos;
        }
    }

    private void MagnetToPlayer()
    {
        if (magnetState == true)
        {
            distanceToPlayer = Vector3.Distance(transform.position, PlayerController.staticController.transform.position);
            if (distanceToPlayer < 1.5f)
            {

                Debug.Log("Follow State: " + magnetState);
                emotionState = false;
                pickUpSpeed = 1.5f - distanceToPlayer;
                transform.position = Vector2.MoveTowards(transform.position, PlayerController.staticController.transform.position, pickUpSpeed * Time.deltaTime);
                tempVal = transform.position.y;
                tempPos = transform.position;
                math = Mathf.Sin(speed * Time.time);
            }
            else
            {
                if ( (Mathf.Sin(speed * Time.time) > 0.1) || (Mathf.Sin(speed * Time.time) < 0.1) )
                {
                    emotionState = true;
                }
                
            }
        }
        
    }



    private void Animotion2()
    {
        if ( onPositionState == true )
        {
            emotionPos = playerT.position + direction * radius;
            if ( transform.position != emotionPos )
            {
                transform.position = Vector3.Slerp(transform.position, emotionPos, Time.deltaTime * 1.5f);
            }
            else
            {
                followState = true;
            }
        }
    }


    private void Animotion3()
    {
        if ( followState == true )
        {
            
        }
    }



    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            switch (emotionColor)
            {
                case EmotionColor.blue: emotionController.SetEmotionWorld(this.gameObject); emotionController.Handle(EmotionColor.blue);      break;
                case EmotionColor.green: emotionController.SetEmotionWorld(this.gameObject); emotionController.Handle(EmotionColor.green);    break;
                case EmotionColor.pink: emotionController.SetEmotionWorld(this.gameObject); emotionController.Handle(EmotionColor.pink);      break;
                case EmotionColor.purple: emotionController.SetEmotionWorld(this.gameObject); emotionController.Handle(EmotionColor.purple);  break;
                case EmotionColor.yellow: emotionController.SetEmotionWorld(this.gameObject); emotionController.Handle(EmotionColor.yellow);  break;
                default: break;
            }
        }
    }









}
