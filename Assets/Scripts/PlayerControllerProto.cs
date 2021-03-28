using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Physics and positioning in the world script for player
public class PlayerControllerProto : MonoBehaviour
{
    // controllers
    public static PlayerControllerProto staticController;
    Animator animator;

    public float speed;
    public float defaultSpeed;
    public float speedModifier;
    Rigidbody2D rigidbody2d;
    private Vector2 lookDirection;
    private Vector2 movement;
    private Vector2 mouseTarget;
    public Vector2 interactiveRayLength = new Vector2(1.5f, 1.5f);
    public Vector2 LookDirection
    {
        get { return lookDirection; }
    }

    public bool needDash = false;
    public int defaultDamage;
    public int damageModifier;
    public int damage;

    private void Awake()
    {
        staticController = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        // GetMouseInput();
        SetLookDirection();

        // Debug.DrawLine(transform.position, mouseTarget, Color.red);
        Debug.DrawRay(transform.position, lookDirection, Color.blue);
        // Debug.Log("Get look direction: " + GetLookDirecton());

        // set animation
        animator.SetFloat("MoveX", lookDirection.x);
        animator.SetFloat("MoveY", lookDirection.y);
        if (movement != Vector2.zero)
            animator.SetFloat("Speed", 1);
        else
            animator.SetFloat("Speed", 0);


        Debug.Log(movement);
    }

    private void FixedUpdate()
    {
        MovementControl();
/*         if (needDash)
            Dash(); */
    }

    private void GetMovementInput()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }

    private void MovementControl()
    {
        Vector2 positionToMove = rigidbody2d.position;
        positionToMove += movement * defaultSpeed * speedModifier * Time.fixedDeltaTime;
        rigidbody2d.MovePosition(positionToMove);
    }

    private void GetMouseInput()
    {
        mouseTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void SetLookDirection()
    {
        if (!Mathf.Approximately(movement.x, 0.0f) || !Mathf.Approximately(movement.y, 0.0f))
        {
            lookDirection.Set(movement.x, movement.y);
            lookDirection.Normalize();
        }
    }

    // Warrior only
    public void Dash()
    {
        rigidbody2d.MovePosition(rigidbody2d.position + movement * Time.fixedDeltaTime * 120);
    }

}