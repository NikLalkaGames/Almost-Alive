using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleEmotion : MonoBehaviour
{
    EmotionController emotionController;

    public EmotionColor emotionColor;
    public float math;


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
    private bool emotionExist = false;

    // find nearest transform value in scene
    // try to magnet to this transform
    // if true, then fix state

    private void Start() 
    {
        if (GetComponentInParent<EmotionController>() != null)  // if parent has emotion controller
        {
            emotionState = false;   // attached as emotion to (as while player) gameObject
            magnetState = false;    // do not use manget functionality

            playerT = GetComponentInParent<PlayerController>().transform;       //player's transform 
            direction = GetComponentInParent<EmotionController>().direction;    //emotion's angle above head
            
            onPositionState = true;    //activate smooth 'above-head transform' sequence 
        }
        emotionController = PlayerController.staticController.transform.GetComponent<EmotionController>();
        tempPos = transform.position;
        tempVal = transform.position.y; 
    }



    private void Update()
    {
        UpDownTransform();
        MagnetToPlayer();
        TransformAboveHead();
    }


    private void UpDownTransform()    //emotion on-ground up-down state  (need to rework)
    {
        if (emotionState == true)  //need to turn-off if another sequence used or emotion attached to another gameobject
        {
            tempPos.y = tempVal + amplitude * Mathf.Sin(speed * Time.time);     //emotion y-coord change by amplitude (length of up-down), mathf.sin calculate up-down position from 0 to 1 
            transform.position = tempPos;                                       
        }
    }


    private void MagnetToPlayer()   //emotion magnet to player
    {
        if (magnetState == true)
        {
            
            foreach (var x in emotionController.emotions)   //check if this emotion already exists in player's pool
            {
                if (x.EmotionColor == this.emotionColor)
                {
                    emotionExist = true;
                }
            }

            distanceToPlayer = Vector3.Distance(transform.position, PlayerController.staticController.transform.position);  //calculate distance to player
            if ((distanceToPlayer < 1.5f) && (emotionExist == false))
            {
                emotionState = false;                       //turn-off UpDownTransform
                pickUpSpeed = 1.5f - distanceToPlayer;      //become faster while distance decreases (like a magnet)
                transform.position = Vector2.MoveTowards(transform.position, PlayerController.staticController.transform.position, pickUpSpeed * Time.deltaTime); //move towards player by pickUpSpeed speed
                tempVal = transform.position.y;         //respond for UpDown transform if magnet sequence interrupted (without it emotion will transform to position where it spawned)
                tempPos = transform.position;             
            }
            else
            {
                emotionState = true;    //turn-on UpDown if magnet sequence interrupted (not works properly)   
            }
        }
        
    }



    private void TransformAboveHead()
    {
        if ( onPositionState == true )  //activate if emotion is player's child
        {
            emotionPos = playerT.position + direction * radius;     //position where emotion supposed to be
            if ( transform.position != emotionPos )
            {
                transform.position = Vector3.Slerp(transform.position, emotionPos, Time.deltaTime * 1.5f);      //transform from player position to emotionPos
            }
            else
            {
                followState = true;     //work-in-progress
            }
        }
    }


    private void Animotion3()       //work-in-progress; smooth following for the player
    {
        if ( followState == true )
        {
            
        }
    }



    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")      //destroy current and spawn player's emotion
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
