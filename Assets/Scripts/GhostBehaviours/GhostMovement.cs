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
        public static GhostMovement Instance { get; private set; } = null;
        
        private Animator _animator;
        
        private EmotionController _emotionController;

        // physics
        [SerializeField] private float defaultSpeed;
        
        [SerializeField] private float speedModifier;
        
        private Rigidbody _rigidbody;

        // sight and movement 
        private RaycastHit _hit;
        
        private Vector3 _lookDirection;
        
        private Vector3 _movement;
        
        private Vector3 _mouseTarget;
        
        private Camera _camera;

        public Vector2 LookDirection => _lookDirection;
        public Vector3 MouseTarget => _mouseTarget;

        # endregion

        # region MonoBehaviours And Methods

        private void Awake()
        {
            if (Instance == null) Instance = this;
            _camera = Camera.main;
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
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
            //_mouseTarget = _camera.ScreenToWorldPoint(Input.mousePosition) ;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out _hit);
            
            // Debug.DrawRay(ray.origin, ray.direction, Color.red);
            //Debug.DrawLine(ray.origin, _hit.point, Color.green);
        }

        private void SetLookDirection()
        {
            _lookDirection = (_hit.point - transform.position).normalized;
        }

        private void FixedUpdate()
        {
            var positionToMove = _rigidbody.position;
            positionToMove += _movement * (defaultSpeed * speedModifier * Time.fixedDeltaTime);
            _rigidbody.MovePosition(positionToMove);
        }

        # endregion
    }
}