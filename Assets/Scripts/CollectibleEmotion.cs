using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class CollectibleEmotion : MonoBehaviour
{

    # region Fields

    // emotion logic variables
    private EmotionController _closestEmotionController;
    public EmotionColor EmotionColor;

    // colliders
    private DetectNearestColliders _colliderDetector;
    private BoxCollider2D _internalCollider;

    // transform logic variables
    private float _distanceToPlayer;
    private Vector3 _direction;
    private Transform _holderTransform;
    private Transform _nearestTransform; 
    private Vector3 _emotionPos;

    //Other variables
    [SerializeField] private float _radius;
    private float _circleRadius;
    private float _pickUpSpeed;
    public float _idleAnimationSpeed;

    //Finite State Machine variables
    public enum States
    {
        Idle, 
        Magnet, 
        AboveHead

    }

    StateMachine<States, StateDriverUnity> fsm;
    
    #endregion

    private void Awake()
    {
        fsm = new StateMachine<States, StateDriverUnity>(this);
        DetectNearestColliders.OnColliderDetectorEnter += OnColliderDetectorEnter;
    }

    private void Start()
    {
        _circleRadius = GetComponentInChildren<CircleCollider2D>().radius * 2;
        _internalCollider = GetComponent<BoxCollider2D>();

        var emotionController = GetComponentInParent<EmotionController>();
        if (emotionController != null)  // if parent has emotion controller (has _holderTransform)
        {
            _holderTransform = emotionController.transform;       // player's transform 
            _direction = emotionController.directionOfAttaching;     // emotion's angle above head
            
            GetComponent<BoxCollider2D>().enabled = false;
            transform.Find("DetectColliders").gameObject.SetActive(false);

            fsm.ChangeState(States.AboveHead);
        }
        else
        {
            _colliderDetector = transform.GetComponentInChildren<DetectNearestColliders>();

            fsm.ChangeState(States.Idle);
        }
    }

     void Idle_FixedUpdate()
    {
        transform.position += new Vector3(0, _idleAnimationSpeed/1000, 0);
    }
     
    void Idle_OnColliderDetectorEnter()
    {
        Debug.Log("Chelik enter in da trigger");
        
        _nearestTransform = Helper.GetClosestTransform(_colliderDetector.GetListOfTriggerTransforms(), transform);
        
        // show list

        _closestEmotionController = _nearestTransform.GetComponent<EmotionController>();

        if ( (_closestEmotionController != null) && (!_closestEmotionController.EmotionExists(EmotionColor) ) )
        {
            fsm.ChangeState(States.Magnet);
        }
    }

    void Magnet_FixedUpdate()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, _nearestTransform.position);   // calculate distance to player

        if ( (_closestEmotionController.EmotionExists(EmotionColor)) || (_distanceToPlayer > _circleRadius) )
        {
            fsm.ChangeState(States.Idle);
        }
        else
        {
            _pickUpSpeed = _circleRadius - _distanceToPlayer;      //become faster while distance decreases
            transform.position = Vector2.MoveTowards(transform.position, _nearestTransform.position, _pickUpSpeed * Time.deltaTime); //move towards player by _pickUpSpeed speed
        }
    }

    void Magnet_OnColliderDetectorEnter()
    {
        if (_internalCollider.IsTouching(_nearestTransform.GetComponent<BoxCollider2D>()))
        {
            switch (EmotionColor)
            {
                case EmotionColor.blue:   _closestEmotionController.SaveEmotionWorld(this.gameObject); _closestEmotionController.Handle(EmotionColor.blue);   break;
                case EmotionColor.green:  _closestEmotionController.SaveEmotionWorld(this.gameObject); _closestEmotionController.Handle(EmotionColor.green);  break;
                case EmotionColor.pink:   _closestEmotionController.SaveEmotionWorld(this.gameObject); _closestEmotionController.Handle(EmotionColor.pink);   break;
                case EmotionColor.purple: _closestEmotionController.SaveEmotionWorld(this.gameObject); _closestEmotionController.Handle(EmotionColor.purple); break;
                case EmotionColor.yellow: _closestEmotionController.SaveEmotionWorld(this.gameObject); _closestEmotionController.Handle(EmotionColor.yellow); break;
                default: break;
            }
        }
    }

    void AboveHead_FixedUpdate()
    {
        
        _emotionPos = _holderTransform.position + _direction * _radius;
        if ( !Helper.Reached(transform.position, _emotionPos) )
        {
            transform.position = Vector2.Lerp(transform.position, _emotionPos, Time.deltaTime * 1.5f);   //transform from player position to emotionPos
        }
    }    

    private void FixedUpdate()
    {
        fsm.Driver.FixedUpdate.Invoke();
    }

    private void OnColliderDetectorEnter()
    {
        fsm.Driver.OnColliderDetectorEnter.Invoke();
    }

    private void OnColliderDetectorExit()
    {
        fsm.Driver.OnColliderDetectorExit.Invoke();
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     fsm.Driver.OnTriggerEnter2D.Invoke(other);
    // }
}