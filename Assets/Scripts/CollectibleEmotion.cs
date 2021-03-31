using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleEmotion : MonoBehaviour
{   
    // emotion logic variables
    EmotionController closestEmotionController;
    public EmotionColor emotionColor;
    DetectNearestColliders colliderDetector;

    // movement logic variables
    public float math;
    public float amplitude;          //Set in Inspector 
    public float speed;                  //Set in Inspector 
    private float tempVal;
    private Vector3 tempPos;
    private float pickUpSpeed;

    // transform logic variables
    private float distanceToPlayer;
    private Vector3 direction;
    private Transform holderTransform;
    private Transform nearestTransform; 
    private Vector3 emotionPos;
    public float radius;

    private bool worldIdleState = true;
    private bool magnetState = true;
    private bool followState = false;
    private bool holderEmotionState = false;

    private bool emotionExist = false;
    private bool foundClosestTransform = false;
    
    private void Start() 
    {
        if (GetComponentInParent<EmotionController>() != null)  // if parent has emotion controller (has holderTransform)
        {
            var emotionController = GetComponentInParent<EmotionController>();      // emotion controller
            worldIdleState = false;             // attached as emotion to (as while player) gameObject
            magnetState = false;                // do not use magnet functionality

            holderTransform = emotionController.transform;       // player's transform 
            direction = emotionController.direction;     // emotion's angle above head
            
            holderEmotionState = true;    //activate smooth 'above-head transform' sequence

            GetComponentInChildren<DetectNearestColliders>().enabled = false;
        }
        else
        {
            colliderDetector = transform.GetComponentInChildren<DetectNearestColliders>();
        }
        tempPos = transform.position;
        tempVal = transform.position.y;
        
    }

    private void Update()
    {
        UpDownTransform();
        MagnetToPlayer();
        TransformAboveHead();
        emotionExist = false;
    }

    private void UpDownTransform()    //emotion on-ground up-down state  (need to rework)
    {
        if (worldIdleState == true)  //need to turn-off if another sequence used or emotion attached to another gameobject
        {
            tempPos.y = tempVal + amplitude * Mathf.Sin(speed * Time.time);     //emotion y-coord change by amplitude (length of up-down), mathf.sin calculate up-down position from 0 to 1 
            transform.position = tempPos;                                       
        }
    }

    private void MagnetToPlayer()   //emotion magnet to player
    {
        if (magnetState == true)
        {
            // if (!foundClosestTransform)    // if closest transform isn't found
            // {
                // state of finding closes transform to magnet
                if ( colliderDetector.GetListOfTriggerColliders().Count != 0)    // if collider list in trigger zone isn't empty
                {                                                                // if trigger zone contain colliders
                    nearestTransform = Helper.GetClosestTransform(colliderDetector.GetListOfTriggerTransforms(), transform);

                    Debug.Log("Closest gameObject: " + nearestTransform.name);
                    
                    closestEmotionController = nearestTransform.GetComponentInChildren<EmotionController>();
                    // foundClosestTransform = true;
                }
            // }
            // else
            // {
                // if closest transform founded try to magnet to it
                if (nearestTransform != null)
                {
                    distanceToPlayer = Vector3.Distance(transform.position, nearestTransform.position);   // calculate distance to player
                    if (distanceToPlayer < 1.5f)
                    {
                        if (closestEmotionController.emotions.Exists(x => x.EmotionColor == this.emotionColor))
                        {
                            emotionExist = true;
                        }

                        if (emotionExist == false)
                        {
                            worldIdleState = false;                       //turn-off UpDownTransform
                            pickUpSpeed = 1.5f - distanceToPlayer;      //become faster while distance decreases (like a magnet)
                            transform.position = Vector2.MoveTowards(transform.position, nearestTransform.position, pickUpSpeed * Time.deltaTime); //move towards player by pickUpSpeed speed
                            tempVal = transform.position.y;         //respond for UpDown transform if magnet sequence interrupted (without it emotion will transform to position where it spawned)
                            tempPos = transform.position;
                        }
                    }
                    else
                    {
                        worldIdleState = true;    //turn-on UpDown if magnet sequence interrupted (not works properly)   
                    }
                }

            // }
        }
        
    }

    private void TransformAboveHead()
    {
        if ( holderEmotionState == true )    //activate if emotion is player's child
        {
            emotionPos = holderTransform.position + direction * radius;   // position where emotion supposed to be
            if ( transform.position != emotionPos )
            {
                transform.position = Vector3.Slerp(transform.position, emotionPos, Time.deltaTime * 1.5f);   //transform from player position to emotionPos
            }
            else
            {
                followState = true;        //work-in-progress
            }
            foundClosestTransform = false; // reset closest transform
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
        if ( !holderEmotionState )
        {
            Debug.Log("Closest emotion controller parent: " + closestEmotionController.transform.parent.tag);
            if (other.CompareTag(closestEmotionController.transform.parent.tag) )      //destroy current and spawn player's emotion
            {
                Debug.Log("After comparing colliders: " + this.name + " with " + other.name);
                switch (emotionColor)
                {
                    case EmotionColor.blue: closestEmotionController.SetEmotionWorld(this.gameObject); closestEmotionController.Handle(EmotionColor.blue); break;
                    case EmotionColor.green: closestEmotionController.SetEmotionWorld(this.gameObject); closestEmotionController.Handle(EmotionColor.green); break;
                    case EmotionColor.pink: closestEmotionController.SetEmotionWorld(this.gameObject); closestEmotionController.Handle(EmotionColor.pink); break;
                    case EmotionColor.purple: closestEmotionController.SetEmotionWorld(this.gameObject); closestEmotionController.Handle(EmotionColor.purple); break;
                    case EmotionColor.yellow: closestEmotionController.SetEmotionWorld(this.gameObject); closestEmotionController.Handle(EmotionColor.yellow); break;
                    default: break;
                }
            }
       } 
    }









}
