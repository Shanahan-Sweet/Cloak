using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour
{

    [SerializeField] InputActionReference jumpAction, moveAction, toolAction;

    //variables
    Vector2 rawMoveAxis, moveAxis;
    public Vector2 MoveAxis { get { return moveAxis; } }
    bool isMoving;
    public bool IsMoving { get { return isMoving; } }

    float timeFromJump, timeFromGrounded;

    //check for ground
    [SerializeField] LayerMask groundMask;
    bool isGrounded;//, roof;
    public bool IsGrounded { get { return isGrounded; } }

    //Ground stabilization
    [SerializeField] PhysicsMaterial2D moveMat, idleMat;

    //Components
    PlayerMovement moveScript;

    Rigidbody2D rigidBody;

    void Awake()
    {
        moveScript = GetComponent<PlayerMovement>();
        rigidBody = GetComponent<Rigidbody2D>();


        //Setup input
        jumpAction.action.Enable();
        moveAction.action.Enable();
        toolAction.action.Enable();
        jumpAction.action.performed += JumpAction;
        jumpAction.action.canceled += JumpCanceled;

        toolAction.action.performed += ToolAction;
    }

    //Update
    private void Update()
    {
        SetMoveAxis(moveAction.action.ReadValue<Vector2>());//get axis input
        CheckForGround();//check for ground
    }

    //Input
    public void JumpAction(InputAction.CallbackContext context)
    {
        PressJump();
    }

    void JumpCanceled(InputAction.CallbackContext context)
    {
        //HoldingJump = false;
    }
    public void ToolAction(InputAction.CallbackContext context)
    {


    }
    void SetMoveAxis(Vector2 newAxis)
    {
        rawMoveAxis = newAxis;
        if (rawMoveAxis.magnitude < .15f)
        {
            isMoving = false;
            moveAxis = Vector2.zero;

            rigidBody.sharedMaterial = idleMat;//stop sliding
            return;
        }
        isMoving = true;
        moveAxis = rawMoveAxis;

        rigidBody.sharedMaterial = moveMat;//no friction

        if (moveAxis.magnitude > 1) moveAxis.Normalize();

        //if (Mathf.Abs(rawMoveAxis.x) > .15f) lastDir = (int)Mathf.Sign(rawMoveAxis.x);//get last look direction
    }

    //Jump
    void PressJump()
    {
        if (Time.timeScale != 0)
        {
            timeFromJump = Time.time + .3f;
        }
    }

    void Jump()//reset jump input
    {
        timeFromGrounded = 0;
        timeFromJump = 0;
        moveScript.MoveJump();
        //effects
        //partJump.Play();
    }



    void CheckForGround()
    {
        if (rigidBody.linearVelocity.magnitude < 4 && Physics2D.OverlapCircle(transform.position + new Vector3(0, -.6f, 0), .15f, groundMask))
        {
            timeFromGrounded = Time.time + .3f;
            if (!isGrounded)
            {
                isGrounded = true;
                //effects
                //partLand.Play();
            }
        }
        else if (isGrounded)//set airborne
        {
            isGrounded = false;
        }

        if (timeFromJump > Time.time && timeFromGrounded > Time.time) Jump();//was close to ground or has pressed jump

        /*
        //check for roof
        if (Physics2D.OverlapCircle(transform.position + new Vector3(0, .5f, 0), .15f, groundMask))
        {
            if (roof == false)
            {
                roof = true;

                //effects
                animScript.Blink(.8f, -1);

                moveScript.Duck();
            }
        }
        else
        {
            roof = false;
        }*/

    }
}
