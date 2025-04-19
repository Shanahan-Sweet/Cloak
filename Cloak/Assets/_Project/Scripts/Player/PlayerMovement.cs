using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{    //state
    public enum PlayerState
    {
        Move,
        Attack,
        Stunned,
        Dead,
    }
    public PlayerState currentState;

    //variables
    [SerializeField] float moveSpd = 25, airMoveSpd = 15, jumpHeight = 7;
    [SerializeField] float slideSpd = 30;


    //dash
    bool isDashing;
    [SerializeField] float dashSpd, dashBurstForce, dashTime;
    IEnumerator dashSequence;
    //Components
    PlayerInput inputScript;
    PlatformerPhysics platformerPhysics;
    Rigidbody2D rigidBody;

    //Debug
    [SerializeField] Transform directionTrans;

    void Awake()
    {
        inputScript = GetComponent<PlayerInput>();
        platformerPhysics = GetComponent<PlatformerPhysics>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case PlayerState.Move: MoveUpdate(); break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case PlayerState.Move: MoveFixed(); break;
            case PlayerState.Attack: AttackFixed(); break;
        }
    }
    //States
    void MoveUpdate()
    {
        /*
            //rotate to target direction
            float rotZ = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            Quaternion newAngle = Quaternion.AngleAxis(rotZ, Vector3.forward);
            float t = 1 - Mathf.Pow(0.5f, Time.deltaTime * 3f);//2.6f
            glideDir.rotation = Quaternion.Lerp(glideDir.rotation, newAngle, t);//Time.deltaTime * 2
            */
    }

    void MoveFixed()
    {
        if (isDashing) return;//dash physics

        if (platformerPhysics.GroundHit && inputScript.MoveAxis.y < -.25f)//holding down
        {
            //MoveSlide();
            MoveWalk();
        }
        else
        {
            MoveWalk();
        }
        //add drag
        ApplyXDrag();

        platformerPhysics.KeepWalkHeight();
        platformerPhysics.FlipUpright(Vector2.up);
    }

    void MoveWalk()
    {
        Vector2 moveDir = GetMoveDirection();
        rigidBody.AddForce(moveDir * inputScript.MoveAxis.x * moveSpd);
        //directionTrans.position = (Vector2)transform.position + moveDir;//debug
    }

    void MoveSlide()
    {
        Vector2 moveDir = GetMoveDirection();
        moveDir *= -Mathf.Sign(moveDir.y);//get down direction

        rigidBody.AddForce(moveDir * slideSpd);

    }

    //Actions
    public void Jump()
    {
        platformerPhysics.Jump(jumpHeight);
    }

    //Attack state
    public void StartAttack()
    {
        currentState = PlayerState.Attack;//set state
    }
    public void AttackStrike(Vector2 attackDir)
    {
        rigidBody.AddForce(attackDir * 4, ForceMode2D.Impulse);
    }
    public void AttackEnded()
    {
        currentState = PlayerState.Move;//reset state
    }

    void AttackFixed()
    {
        // rigidBody.AddForce(new Vector2(inputScript.MoveAxis.x * moveSpd, 0));

        Vector2 moveDir = GetMoveDirection();
        rigidBody.AddForce(moveDir * inputScript.MoveAxis.x * moveSpd * .1f);
        directionTrans.localPosition = moveDir;
        //add drag
        rigidBody.AddForce(new Vector2(-rigidBody.linearVelocity.x * (platformerPhysics.IsGrounded ? 8 : .5f), 0));//X drag


        platformerPhysics.KeepWalkHeight();
        platformerPhysics.FlipUpright(Vector2.up);
    }

    public void StartDash()
    {
        isDashing = true;
        if (dashSequence != null) StopCoroutine(dashSequence);
        dashSequence = DashSequence();
        StartCoroutine(dashSequence);
    }

    void ForceEndDash()
    {
        isDashing = false;
        if (dashSequence != null) StopCoroutine(dashSequence);
    }

    IEnumerator DashSequence()
    {
        Vector2 dashDir = GetMoveDirection() * inputScript.GetlastXDir;
        directionTrans.localPosition = dashDir;//debug

        rigidBody.linearVelocity = dashDir * dashBurstForce;
        platformerPhysics.Jump(3);

        float t = 0;
        while (t < dashTime)
        {
            t += Time.fixedDeltaTime;

            rigidBody.AddTorque(-inputScript.GetlastXDir * 5);
            //rigidBody.AddForce(dashDir * dashSpd);
            //ApplyXDrag(8);



            yield return new WaitForFixedUpdate();
        }

        isDashing = false;
    }


    //Forces
    void ApplyXDrag(float drag = 8)
    {
        rigidBody.AddForce(new Vector2(-rigidBody.linearVelocity.x * drag, 0));//X drag
    }

    //walk direction
    Vector2 GetMoveDirection()
    {
        return platformerPhysics.GroundHit ? Quaternion.AngleAxis(-90, Vector3.forward) * platformerPhysics.GroundNormal : Vector2.right;
    }
}
