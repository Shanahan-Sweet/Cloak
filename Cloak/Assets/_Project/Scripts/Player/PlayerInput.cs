using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour
{

    [SerializeField] InputActionReference jumpAction, moveAction;

    //variables
    Vector2 rawMoveAxis, moveAxis;
    float lastXDir;
    public Vector2 MoveAxis { get { return moveAxis; } }
    public float GetlastXDir { get { return lastXDir; } }
    bool isMoving;
    public bool IsMoving { get { return isMoving; } }

    //Components
    PlayerMovement moveScript;

    Rigidbody2D rigidBody;

    //Animation
    //[Header("Animation")]

    void Awake()
    {
        moveScript = GetComponent<PlayerMovement>();
        rigidBody = GetComponent<Rigidbody2D>();


        //Setup input
        jumpAction.action.Enable();
        moveAction.action.Enable();
        jumpAction.action.performed += JumpAction;
        jumpAction.action.canceled += JumpCanceled;
    }

    //Update
    private void Update()
    {
        SetMoveAxis(moveAction.action.ReadValue<Vector2>());//get axis input


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

    void SetMoveAxis(Vector2 newAxis)
    {
        rawMoveAxis = newAxis;

        if (Mathf.Abs(rawMoveAxis.x) > 0.1f) lastXDir = Mathf.Sign(rawMoveAxis.x);//save last held direction

        if (rawMoveAxis.magnitude < .15f)
        {
            isMoving = false;
            moveAxis = Vector2.zero;

            return;
        }
        isMoving = true;
        moveAxis = rawMoveAxis;


        if (moveAxis.magnitude > 1) moveAxis.Normalize();

        //if (Mathf.Abs(rawMoveAxis.x) > .15f) lastDir = (int)Mathf.Sign(rawMoveAxis.x);//get last look direction
    }

    //Jump
    void PressJump()
    {
        if (Time.timeScale == 0) return;

        moveScript.PressJump();

    }

    void Jump()//reset jump input
    {

    }

}