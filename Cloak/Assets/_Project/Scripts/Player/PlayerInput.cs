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
    bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }

    //ground raycasts
    GroundPoint groundHit = new GroundPoint();
    public bool GroundHit { get { return groundHit.groundHit; } }
    public Vector2 GroundPoint { get { return groundHit.groundPoint; } }
    public Vector2 GroundNormal { get { return groundHit.groundNormal; } }
    [SerializeField] Transform[] groundPointTransform;

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
        CheckGroundRaycast();
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
            timeFromJump = Time.time + .2f;
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
        if (rigidBody.linearVelocity.magnitude < 4 && Physics2D.OverlapCircle(transform.position + new Vector3(0, -.5f, 0), .1f, groundMask))
        {
            timeFromGrounded = Time.time + .15f;
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

    }

    void CheckGroundRaycast()
    {
        float groundCheckDist = .8f;
        GroundPoint leftHit = DownRay((Vector2)transform.position - Vector2.right * .1f, groundCheckDist);
        GroundPoint rightHit = DownRay((Vector2)transform.position + Vector2.right * .1f, groundCheckDist);

        groundHit.groundHit = leftHit.groundHit && rightHit.groundHit;

        if (!leftHit.groundHit)
            groundHit = rightHit;
        else if (!rightHit.groundHit)
            groundHit = leftHit;
        else
        {
            groundHit.groundPoint = (leftHit.groundPoint + rightHit.groundPoint) / 2;
            groundHit.groundNormal = (leftHit.groundNormal + rightHit.groundNormal) / 2;
            groundHit.groundNormal.Normalize();
        }


        groundPointTransform[0].position = leftHit.groundPoint;
        groundPointTransform[1].position = groundHit.groundPoint;
        groundPointTransform[2].position = rightHit.groundPoint;
    }

    GroundPoint DownRay(Vector2 origin, float length)
    {
        GroundPoint hitData = new GroundPoint();
        hitData.groundHit = false;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, length, groundMask);
        if (hit.collider)
        {
            hitData.groundHit = true;
            hitData.groundPoint = hit.point;
            hitData.groundNormal = hit.normal;
        }

        return hitData;
    }
}

public class GroundPoint
{
    public bool groundHit;
    public Vector2 groundPoint;
    public Vector2 groundNormal;
}