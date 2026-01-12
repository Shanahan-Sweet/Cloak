using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState
    {
        Move,
        Climb,
        Boost,
        Stun
    }
    PlayerState currentState;
    float stateTimer;

    [SerializeField] float moveForce = 1, jumpSpd = 2, boostImpulseForce = 4, boostForce;
    bool holdingJump, chargingJump = false, jumpCharged;

    float jumpCharge = 0;


    [SerializeField] float climbSpd = 4;
    float defaultDrag, defaultAngularDrag;
    float timeFromClimb;
    [SerializeField] WallDetection wallDetection;
    [SerializeField] LayerMask climbSurfaceMask;

    //boost
    Vector2 boostDirection;
    float boostT;

    //components
    PlayerInput inputScript;
    PlayerAnimation animScript;
    Rigidbody2D rigidBody;

    void Awake()
    {
        inputScript = GetComponent<PlayerInput>();
        animScript = GetComponent<PlayerAnimation>();
        rigidBody = GetComponent<Rigidbody2D>();
        defaultDrag = rigidBody.linearDamping;
        defaultAngularDrag = rigidBody.angularDamping;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case PlayerState.Move: MoveFloatFixed(); break;
            case PlayerState.Climb: MoveClimbFixed(); break;
            case PlayerState.Boost: MoveBoostFixed(); break;
            case PlayerState.Stun: StunFixed(); break;
        }
    }

    void SetMove()
    {
        currentState = PlayerState.Move;
        animScript.SetMove();
        rigidBody.linearDamping = defaultDrag;
        rigidBody.angularDamping = defaultAngularDrag;
    }

    void MoveFloatFixed()
    {
        Vector2 moveAxis = chargingJump ? Vector2.zero : inputScript.GetRawAxis;
        rigidBody.AddForce(moveAxis * moveForce);

        if (moveAxis.magnitude > .25f)
            RotateToDirection(transform.up, moveAxis.normalized, .4f);//float rotation

        //check if climbing
        bool checkClimb = Time.time > timeFromClimb && inputScript.GetRawAxis.magnitude > .5f;//is holding direction
        if (checkClimb && Physics2D.OverlapCircle(transform.position, .1f, climbSurfaceMask))
        {
            SetClimb();
            rigidBody.AddTorque(Mathf.Sign(Random.Range(-1, 1)) * .1f, ForceMode2D.Impulse);//spin when grabbing surface
            animScript.PlayGrubParticles();
        }
        ChargeFixedUpdate();//charge boost
        CheckJumpShake();
    }

    void SetClimb()
    {
        currentState = PlayerState.Climb;
        animScript.SetClimb();
        rigidBody.linearDamping = 8;
        rigidBody.angularDamping = 3;
    }
    void MoveClimbFixed()
    {
        Vector2 moveAxis = chargingJump ? Vector2.zero : inputScript.GetRawAxis;
        rigidBody.AddForce(moveAxis * climbSpd);

        if (moveAxis.magnitude > .25f)
            RotateToDirection(transform.up, moveAxis.normalized, .1f);//climb rotation

        if (!Physics2D.OverlapCircle(transform.position, .1f, climbSurfaceMask))
        {
            timeFromClimb = Time.time + .1f;
            SetMove();

            rigidBody.AddTorque(Mathf.Sign(Random.Range(-1, 1)) * .2f, ForceMode2D.Impulse);//spin when letting go

            animScript.PlaySmallGrubParticles();
        }
        ChargeFixedUpdate();//charge boost
        CheckJumpShake();
    }

    void SetBoost(Vector2 dir)
    {
        boostT = Time.time + .75f;
        currentState = PlayerState.Boost;
        boostDirection = dir;
        //animScript.SetBoost();
        rigidBody.linearDamping = 1;//drag
        rigidBody.angularDamping = defaultAngularDrag;

        rigidBody.AddForce(inputScript.GetRawAxis * boostImpulseForce, ForceMode2D.Impulse);
        rigidBody.AddTorque(Mathf.Sign(Random.Range(-1, 1)) * .2f, ForceMode2D.Impulse);//spin when boosting

        animScript.PlaySmallGrubParticles();
    }

    void MoveBoostFixed()//Boost!
    {
        rigidBody.AddForce(boostDirection * boostForce);

        //if (moveAxis.magnitude > .25f)
        //RotateToDirection(transform.up, moveAxis.normalized, .4f);//float rotation

        //check if climbing
        bool checkClimb = Time.time > boostT + .15f && inputScript.GetRawAxis.magnitude > .5f;//is holding direction
        if (checkClimb && Physics2D.OverlapCircle(transform.position, .1f, climbSurfaceMask))//cancel boost
        {
            SetClimb();
            rigidBody.AddTorque(Mathf.Sign(Random.Range(-1, 1)) * .1f, ForceMode2D.Impulse);//spin when grabbing surface
            animScript.EndBoost();
            animScript.PlayGrubParticles();
            return;
        }

        if (boostT < Time.time)//end boost
        {
            animScript.EndBoost();
            SetMove();
        }
    }

    //___________________________Input
    public void PressJump()
    {
        holdingJump = true;
    }

    public void ReleaseJump()
    {
        holdingJump = false;
        Jump(jumpCharged);
    }
    void ChargeFixedUpdate()
    {
        if (holdingJump)
        {
            if (chargingJump == false)
            {
                StartChargingJump();//start charging
            }

            jumpCharge = Mathf.Clamp01(jumpCharge += Time.fixedDeltaTime);
            if (jumpCharge > .9f && !jumpCharged)
            {
                ChargedJump();
            }
        }
    }
    void StartChargingJump()
    {
        jumpCharged = false;
        chargingJump = true;
        jumpCharge = 0;
        if (currentState == PlayerState.Move)
        {
            rigidBody.linearDamping = 1.5f;//charging drag
        }
        //effects
        animScript.StartChargingJump();
    }
    public void ChargedJump()
    {
        jumpCharged = true;
        //effects
        animScript.ChargeJump();
    }

    void CancelCharge()//reset
    {
        jumpCharged = false;
        chargingJump = false;
    }

    public void Jump(bool charged)
    {
        //effects

        animScript.SetShake(false);

        chargingJump = false;
        jumpCharged = false;

        if (charged && inputScript.GetRawAxis.magnitude > .25f)//floating impulse
        {
            animScript.ReleaseJump(true);
            SetBoost(inputScript.GetRawAxis);

            return;
        }


        animScript.ReleaseJump(false);//not boosting effect


        if (currentState == PlayerState.Climb)//end climb state
        {
            timeFromClimb = Time.time + .4f;
            SetMove();

            Vector2 jumpDir2 = inputScript.GetRawAxis;
            rigidBody.linearVelocity = jumpDir2 * jumpSpd;

            animScript.PlayGrubParticles();
            return;
        }
        rigidBody.linearDamping = defaultDrag;



        //wall jump
        if (wallDetection.CurrentCollisionCount != 0)
        {
            animScript.PlayGrubParticles();
            Vector2 jumpDir = (wallDetection.JumpDir + inputScript.GetRawAxis * 1.2f).normalized;
            rigidBody.linearVelocity = jumpDir * jumpSpd;

            if (wallDetection.KickObj != null)
            {
                wallDetection.KickObj.Kick(-wallDetection.JumpDir);//kick physics objects away
            }
        }
        //rigidBody.AddForce(jumpDir * jumpSpd, ForceMode2D.Impulse);
    }

    //Stun
    public void SetStun(Vector2 force, float stunDuration)
    {
        currentState = PlayerState.Stun;
        stateTimer = Time.time + stunDuration;

        rigidBody.linearDamping = defaultDrag;
        rigidBody.angularDamping = defaultAngularDrag;

        if (force.magnitude > 1) rigidBody.linearVelocity = force;

        //effects
        animScript.SetStun();
    }

    void StunFixed()
    {
        if (stateTimer < Time.time)//end stun
        {
            SetMove();
        }
    }




    void RotateToDirection(Vector2 fromDir, Vector2 toDir, float speed)
    {

        rigidBody.AddTorque(Quaternion.FromToRotation(fromDir, toDir).z * speed);//rotate
    }


    //Effects
    void CheckJumpShake()
    {
        animScript.SetShake(jumpCharged && inputScript.GetRawAxis.magnitude > .25f);
    }
}
