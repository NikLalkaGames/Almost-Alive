using System.Collections;
using System.Text;
using Core.Helpers;
using UnityEngine;
using MonsterLove.StateMachine;
using Core.StateMachine.Driver;

// Physics and positioning in the world script for player
namespace GhostBehaviours
{
    public class GhostMovement : MonoBehaviour
    {
        # region Fields

        // State Machine
        public enum States
        {
            Init,
            Movement,
            Attack
        }
        
        private StateMachine<States, Driver> _fsm;
        
        // controllers
        public static GhostMovement Instance { get; private set; } = null;
        
        private Animator _animator;
        
        // physics
        [SerializeField] private float defaultSpeed;
        
        [SerializeField] private float speedModifier;
        
        private Rigidbody _rigidbody;

        private Transform _transform;
        

        // sight and movement 
        private RaycastHit _hit;
        
        [SerializeField]
        private Vector2 _lookDirection;
        
        private Vector3 _movement;
        
        private Vector3 _mouseTarget;
        
        private Camera _camera;

        public Vector2 LookDirection => _lookDirection;
        public Vector3 MouseTarget => _mouseTarget;
        
        // state triggers
        private bool _movementTrigger;

        private bool _attackTrigger;

        // attack time
        [SerializeField] private float attackTime;       // 0.6f
        
        # endregion

        # region StateMachine implementation
        
        #region Init
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            _transform = transform;
            _camera = Camera.main;
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
            _fsm = new StateMachine<States, Driver>(this);
            _fsm.ChangeState(States.Init);
        }
        
        private void Init_Enter()
        {
            Debug.Log("Player Idle");
        }

        private void Init_Update()
        {
            if (_movementTrigger)
                _fsm.ChangeState(States.Movement);
            else if (_attackTrigger)
                _fsm.ChangeState(States.Attack);
        }
        
        #endregion

        #region Movement

        private void Movement_Enter()
        {
            Debug.Log("Enter PlayerGhost movement");
        }
        
        private void Movement_Update()
        {
            // animation logic
            _animator.SetFloat("MoveX", _movement.x);
            _animator.SetFloat("MoveY", _movement.y);
            _animator.SetFloat("speed", _movement.magnitude);
            
            if (!_movementTrigger)
                _fsm.ChangeState(States.Init);
            else if (_attackTrigger)
                _fsm.ChangeState(States.Attack);
        }

        private void Movement_FixedUpdate()
        {
            var positionToMove = _rigidbody.position;
            positionToMove += _movement * (defaultSpeed * speedModifier * Time.fixedDeltaTime);
            _rigidbody.MovePosition(positionToMove);
        }

        #endregion
        
        #region Attack

        private IEnumerator Attack_Enter()
        {
            Debug.Log("Player Attack Enter");
            
            _animator.SetTrigger("AttackTrigger");
            _animator.SetFloat("AttackX", _lookDirection.x);
            _animator.SetFloat("AttackY", _lookDirection.y);
            
            yield return new WaitForSeconds(attackTime);

            _fsm.ChangeState(_movementTrigger ? States.Movement : States.Init);
        }

        private void Attack_Exit()
        {
            _attackTrigger = false;
        }
        
        #endregion
        
        #region StateMachine Drivers
        
        #region Methods
        
        private void GetMovementInput()
        {
            _movement.x = Input.GetAxis("Horizontal");
            _movement.y = Input.GetAxis("Vertical");

            _movementTrigger = !Helpers.Reached(_movement, Vector3.zero);
        }

        private void GetMouseInput()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out _hit);

            if (Input.GetButtonDown("Fire1"))
            {
                _attackTrigger = true;
            }
        }

        private void SetLookDirection()
        {
            _lookDirection = (_hit.point - transform.position).normalized;
            Debug.DrawRay(_transform.position, _lookDirection, Color.blue);
        }

        #endregion
        
        private void Update()
        {
            GetMovementInput();
            GetMouseInput();
            SetLookDirection();             // based on mouse input
            _fsm.Driver.Update.Invoke();
        }

        private void FixedUpdate()
        {
            _fsm.Driver.FixedUpdate.Invoke();
        }
        
        #endregion

        # endregion
    }
}