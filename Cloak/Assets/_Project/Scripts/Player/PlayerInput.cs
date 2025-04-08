using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour, IWeaponMaster
{

    [SerializeField] InputActionReference jumpAction, moveAction, toolAction;

    //variables
    Vector2 rawMoveAxis, moveAxis;
    public Vector2 MoveAxis { get { return moveAxis; } }
    bool isMoving;
    public bool IsMoving { get { return isMoving; } }

    float timeFromJump;

    //Ground stabilization
    [SerializeField] PhysicsMaterial2D moveMat, idleMat;

    //Components
    PlayerMovement moveScript;
    PlatformerPhysics platformerPhysics;
    Rigidbody2D rigidBody;
    [SerializeField] Weapon myWeapon;

    //Animation
    [Header("Animation")]
    [SerializeField] Avatar myAvatar;

    void Awake()
    {
        moveScript = GetComponent<PlayerMovement>();
        platformerPhysics = GetComponent<PlatformerPhysics>();
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

        platformerPhysics.CheckForGround();//check for ground
        platformerPhysics.CheckGroundRaycast();
        platformerPhysics.CheckCelingRaycast();
        platformerPhysics.ChangeStandHeight(moveAxis.y > 0 ? moveAxis.y * .1f : moveAxis.y * .2f);
        if (timeFromJump > Time.time && platformerPhysics.TimeFromGrounded > Time.time) Jump();//was close to ground or has pressed jump

        //animation
        myAvatar.SetInputAxis(moveAxis);
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
        StartAttack();
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
        timeFromJump = 0;
        moveScript.Jump();
        //effects
        //partJump.Play();
    }


    //Attacks
    void StartAttack()
    {
        myWeapon.StartAttack(moveAxis);
        moveScript.StartAttack();
    }

    public void AttackStrike(Vector2 attackDir)
    {
        moveScript.AttackStrike(attackDir);
    }
    public void AttackEnded()
    {
        moveScript.AttackEnded();
    }

}