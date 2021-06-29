using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Physics and positioning in the world script for player
public class GhostMovement : MonoBehaviour
{
    # region Fields
    
    // controllers
    public static GhostMovement instance { get; set; }
    private Animator _animator;
    private EmotionController _emotionController;

    // physics
    [SerializeField] private float _speed;
    [SerializeField] private float _defaultSpeed;
    [SerializeField] private float _speedModifier;
    Rigidbody2D _rigidbody2d;

    // sight and movement 
    private Vector2 _lookDirection;
    private Vector2 _movement;
    private Vector2 _mouseTarget;

    public Vector2 LookDirection =>_lookDirection;
    public Vector2 MouseTarget => _mouseTarget;

    # endregion

    # region MonoBehaviours And Methods

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _emotionController = GetComponentInChildren<EmotionController>();
    }

    void Update()
    {
        GetMovementInput();
        GetMouseInput();
        SetLookDirection(); // based on mouse input

        // TODO: move later emotion drop to emotion controller 
        // emotion drop
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _emotionController.Handle(EmotionColor.none);
        }

        // Debug.DrawLine(transform.position, mouseTarget, Color.red);
        Debug.DrawRay(transform.position, _lookDirection, Color.red);

        // animation logic
        _animator.SetFloat("MoveX", _movement.x);
        _animator.SetFloat("MoveY", _movement.y);
        _animator.SetFloat("speed", _movement.magnitude);

    }

    private void GetMovementInput()
    {
        _movement.x = Input.GetAxis("Horizontal");
        _movement.y = Input.GetAxis("Vertical");
    }

    private void GetMouseInput()
    {
        _mouseTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void SetLookDirection()
    {
        _lookDirection = (_mouseTarget - _rigidbody2d.position).normalized;
    }

    private void FixedUpdate()
    {
        Vector2 positionToMove = _rigidbody2d.position;
        positionToMove += _movement * _defaultSpeed * _speedModifier * Time.fixedDeltaTime;
        _rigidbody2d.MovePosition(positionToMove);
    }

    # endregion
}