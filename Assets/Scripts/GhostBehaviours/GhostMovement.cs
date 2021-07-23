using Emotions.Controllers;
using UnityEngine;
using UnityEngine.Serialization;

// Physics and positioning in the world script for player
namespace GhostBehaviours
{
    public class GhostMovement : MonoBehaviour
    {
        # region Fields
    
        // controllers
        public static GhostMovement Instance { get; set; }
        private Animator _animator;
        private EmotionController _emotionController;

        // physics
        [SerializeField] private float speed;
        [SerializeField] private float defaultSpeed;
        [SerializeField] private float speedModifier;
        private Rigidbody2D _rigidbody2d;

        // sight and movement 
        private Vector2 _lookDirection;
        private Vector2 _movement;
        private Vector2 _mouseTarget;

        public Vector2 LookDirection => _lookDirection;
        public Vector2 MouseTarget => _mouseTarget;

        # endregion

        # region MonoBehaviours And Methods

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _rigidbody2d = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _emotionController = GetComponentInChildren<EmotionController>();
        }

        private void Update()
        {
            GetMovementInput();
            GetMouseInput();
            SetLookDirection(); // based on mouse input

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
            positionToMove += _movement * (defaultSpeed * speedModifier * Time.fixedDeltaTime);
            _rigidbody2d.MovePosition(positionToMove);
        }

        # endregion
    }
}