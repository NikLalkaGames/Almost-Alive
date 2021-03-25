using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Physics and positioning in the world script for player
public class PlayerController : MonoBehaviour
{
    // controllers
    public static PlayerController staticController;
    Animator animator;
    EmotionController emotionController;

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
    public Vector2 MouseTarget
    {
        get { return mouseTarget; }
    }

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
        emotionController = GetComponent<EmotionController>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        GetMouseInput();
        SetLookDirection(); // based on mouse input
        ListenInteractByMouseClick();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            emotionController.Handle(EmotionColor.none);
        }

        //Debug.DrawLine(transform.position, mouseTarget, Color.red);
        Debug.DrawRay(transform.position, lookDirection, Color.blue);

        // animation logic
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetFloat("speed", movement.magnitude);

    }

    private void FixedUpdate()
    {
        MovementUpdate();
    }

    private void GetMovementInput()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }

    private void MovementUpdate()
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
        lookDirection = (mouseTarget - rigidbody2d.position).normalized;

/*         // set look direction
        if (!Mathf.Approximately(movement.x, 0.0f) || !Mathf.Approximately(movement.y, 0.0f))
        {
            lookDirection.Set(movement.x, movement.y);
            lookDirection.Normalize();
        } */
    }

    private void ListenInteractByMouseClick()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1f, LayerMask.GetMask("Consumable"));
            if (hit.collider != null)
            {
                Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
                ConsumableBehaviour littleMan = hit.collider.GetComponent<ConsumableBehaviour>();
                if (littleMan != null)
                {
                    littleMan.Kill();
                }
            }
        }
    }

    //
}