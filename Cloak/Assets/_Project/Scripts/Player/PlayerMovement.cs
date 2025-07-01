using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState
    {
        Move, Jump
    }
    PlayerState currentState;
    float stateTimer;
    [Header("Variables")]
    [SerializeField] float walkSpd = 8, jumpSpd;

    //jump
    bool isAirborne = false;
    [SerializeField] Transform jumpHolder;
    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpHeight = .5f, jumpAnimSpeed = 6;
    [SerializeField] Animator jumpAnim;
    Vector2 jumpDir;
    IEnumerator jumpSequence;

    [Header("GroundDetection")]
    [SerializeField] EdgeColliders edgeCollider;
    //components
    PlayerInput inputScript;
    Rigidbody2D rigidBody;
    BasePlatformer platformerScript;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        inputScript = GetComponent<PlayerInput>();
        platformerScript = GetComponent<BasePlatformer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //States
    void FixedUpdate()
    {
        switch (currentState)
        {
            case PlayerState.Move: MoveFixed(); break;
            case PlayerState.Jump: JumpFixed(); break;

            default: MoveFixed(); break;
        }
    }

    void MoveFixed()
    {
        Vector2 direction = inputScript.MoveAxis;
        //direction = platformerScript.CheckGroundedDirection(transform.position, .5f, direction);//don't walk off edges

        rigidBody.AddForce(direction * walkSpd);

        if (!isAirborne && !platformerScript.IsAboveGround(transform.position, .05f) && direction.magnitude == 0)
        {
            transform.position = Vector3.zero;
        }
    }

    //jump
    void SetAirborne(bool value)
    {
        isAirborne = value;
        edgeCollider.SetCollidersActive(!value);
    }
    void JumpFixed()
    {
        rigidBody.AddForce(jumpDir * jumpSpd);
        Vector2 direction = inputScript.MoveAxis;
        rigidBody.AddForce(direction * walkSpd);

        if (stateTimer < Time.time)//end jump state
        {
            currentState = PlayerState.Move;
            edgeCollider.SetCollidersActive(true);
            EndJump();
            return;
        }
    }

    void EndJump()
    {
        StartlandAnim();
    }

    //Input
    public void PressJump()
    {
        if (currentState == PlayerState.Jump || isAirborne) return;
        //change state
        currentState = PlayerState.Jump;
        stateTimer = Time.time + .3f;


        jumpDir = inputScript.MoveAxis;
        rigidBody.AddForce(jumpDir * 5, ForceMode2D.Impulse);

        //animation
        jumpAnim.SetTrigger("Jump");
        StartJumpAnim();
    }

    void StartJumpAnim()
    {
        if (jumpSequence != null) StopCoroutine(jumpSequence);
        jumpSequence = JumpAnim();

        StartCoroutine(jumpSequence);
    }

    void StartlandAnim()
    {
        if (jumpSequence != null) StopCoroutine(jumpSequence);
        jumpSequence = LandAnim();

        StartCoroutine(jumpSequence);
    }

    IEnumerator JumpAnim()
    {
        SetAirborne(true);
        float value = 0;
        while (value < 1)
        {
            value += Time.deltaTime * jumpAnimSpeed;
            float animValue = jumpCurve.Evaluate(value);
            float height = Mathf.Lerp(0, jumpHeight, animValue);
            jumpHolder.localPosition = new Vector3(0, height, 0);

            yield return null;
        }
    }

    IEnumerator LandAnim()
    {
        float value = 0;
        while (value < 1)
        {
            value += Time.deltaTime * jumpAnimSpeed;
            float animValue = jumpCurve.Evaluate(1 - value);
            float height = Mathf.Lerp(0, jumpHeight, animValue);
            jumpHolder.localPosition = new Vector3(0, height, 0);
            yield return null;
        }
        SetAirborne(false);
        jumpAnim.SetTrigger("Land");
    }
}
