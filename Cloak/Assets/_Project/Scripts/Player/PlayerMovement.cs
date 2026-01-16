using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState
    {
        Move,
        Climb,
        Boost,
        Stun,
        PowerSocket
    }
    PlayerState currentState;
    float stateTimer;

    bool wallMovement = false;

    [SerializeField] float moveForce = 1, jumpSpd = 2, boostImpulseForce = 4, boostForce;// smallImpulseForce = .5f, 
    bool holdingJump, chargingJump = false, jumpCharged;
    float chargeDrag = 1.5f;
    float jumpCharge = 0;


    [SerializeField] float climbSpd = 4;
    float defaultDrag, defaultAngularDrag;
    [SerializeField] float wallDrag, wallMoveForce;
    float timeFromClimb;
    [SerializeField] WallDetection wallDetection;
    bool wallDetected = false;
    [SerializeField] LayerMask climbSurfaceMask;

    //boost
    Vector2 boostDirection;
    float boostT;

    //Power Socket
    Transform socketTrans;
    PowerSocket powerSocket;

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
            case PlayerState.PowerSocket: PowerSocketFixed(); break;
        }
    }

    void SetMove()
    {
        currentState = PlayerState.Move;
        animScript.SetMove();
        rigidBody.linearDamping = wallDetected ? wallDrag : defaultDrag;
        rigidBody.angularDamping = defaultAngularDrag;

        EndPoweredState();
    }

    void MoveFloatFixed()
    {
        Vector2 moveAxis = chargingJump ? Vector2.zero : inputScript.GetRawAxis;


        UpdateMoveState();
        if (wallMovement)
        {
            rigidBody.AddForce(moveAxis * wallMoveForce);//wall movement force
            rigidBody.AddForce(-wallDetection.JumpDir * .5f);//wall force
        }
        else
        {
            rigidBody.AddForce(moveAxis * moveForce);//float force
        }
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

    public void UpdateWallDetection(bool isColliding)
    {
        wallDetected = isColliding;
    }

    void UpdateMoveState()
    {
        if (wallMovement)//current state = wall movement
        {
            if (chargingJump || !wallDetected || inputScript.GetRawAxis.magnitude <= .25f)
            {
                wallMovement = false;
                rigidBody.linearDamping = chargingJump ? chargeDrag : defaultDrag;
            }
        }
        else//check wall movement
        {
            if (!chargingJump && wallDetected && inputScript.GetRawAxis.magnitude > .25f)
            {
                wallMovement = true;
                rigidBody.linearDamping = wallDrag;
            }
        }
    }

    void SetClimb()
    {
        currentState = PlayerState.Climb;
        animScript.SetClimb();
        rigidBody.linearDamping = 8;
        rigidBody.angularDamping = 3;

        EndPoweredState();
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
        EndPoweredState();
        boostDirection = dir;
        //animScript.SetBoost();
        rigidBody.linearDamping = 1;//drag
        rigidBody.angularDamping = defaultAngularDrag;

        rigidBody.AddForce(inputScript.GetRawAxis * boostImpulseForce, ForceMode2D.Impulse);
        rigidBody.AddTorque(Mathf.Sign(Random.Range(-1, 1)) * .2f, ForceMode2D.Impulse);//spin when boosting

        animScript.PlaySmallGrubParticles();

        //screen shake
        MainCam.instance.Shake(.2f, .03f);
    }

    void MoveBoostFixed()//Boost!
    {
        rigidBody.AddForce(boostDirection * boostForce);

        //if (moveAxis.magnitude > .25f)
        //RotateToDirection(transform.up, moveAxis.normalized, .4f);//float rotation

        //check if climbing
        bool checkClimb = Time.time > boostT - .3f && inputScript.GetRawAxis.magnitude > .5f;//is holding direction
        if (checkClimb && Physics2D.OverlapCircle(transform.position, .1f, climbSurfaceMask))//cancel boost
        {
            SetClimb();
            rigidBody.AddTorque(Mathf.Sign(Random.Range(-1, 1)) * .1f, ForceMode2D.Impulse);//spin when grabbing surface
            animScript.EndBoost();
            animScript.PlayGrubParticles();
            return;
        }

        if (boostT < Time.time + (holdingJump ? .5f : 0))//end boost
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
            rigidBody.linearDamping = chargeDrag;//charging drag
        }
        //effects
        animScript.StartChargingJump();
    }
    public void ChargedJump()
    {
        jumpCharged = true;
        //effects
        animScript.ChargeJump();

        //screen shake
        MainCam.instance.Shake(.1f, .01f);
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

        if (charged && inputScript.GetRawAxis.magnitude > .25f)//charged boost
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

        /*if (inputScript.GetRawAxis.magnitude > .25f)//floating impulse
        {
            rigidBody.AddForce(inputScript.GetRawAxis * smallImpulseForce, ForceMode2D.Impulse);
            animScript.PlayGrubParticles();
        }*/

        //wall jump
        /*if (wallDetection.CurrentCollisionCount != 0)
        {
            animScript.PlayGrubParticles();
            Vector2 jumpDir = (wallDetection.JumpDir + inputScript.GetRawAxis * 1.2f).normalized;
            rigidBody.linearVelocity = jumpDir * jumpSpd;

            if (wallDetection.KickObj != null)
            {
                wallDetection.KickObj.Kick(-wallDetection.JumpDir);//kick physics objects away
            }
        }*/
        //rigidBody.AddForce(jumpDir * jumpSpd, ForceMode2D.Impulse);
    }

    //Stun
    public void SetStun(Vector2 force, float stunDuration)
    {
        currentState = PlayerState.Stun;
        stateTimer = Time.time + stunDuration;
        EndPoweredState();

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

    //owerSocket
    public void SocketCollision(PowerSocket newSocket, Transform newSocketTrans)
    {
        if (currentState != PlayerState.PowerSocket)
        {
            SetPowerSocket(newSocket, newSocketTrans);
        }

    }
    void SetPowerSocket(PowerSocket newSocket, Transform newSocketTrans)
    {
        currentState = PlayerState.PowerSocket;
        socketTrans = newSocketTrans;
        powerSocket = newSocket;
        powerSocket.SetPowered();

        rigidBody.linearDamping = 4;
        rigidBody.angularDamping = defaultAngularDrag;


        //effects
        animScript.SetMove();
        animScript.EndBoost();
    }

    void PowerSocketFixed()
    {
        if (socketTrans == null)//end state
        {

            SetMove();
            return;
        }
        Vector2 dir = socketTrans.position - transform.position;
        float moveStrength = Mathf.InverseLerp(0, .2f, dir.magnitude);
        rigidBody.AddForce(dir.normalized * 20 * moveStrength);

        Vector2 moveAxis = chargingJump ? Vector2.zero : inputScript.GetRawAxis;

        RotateToDirection(transform.up, moveAxis.normalized, .2f);//rotation

        ChargeFixedUpdate();//charge boost
        CheckJumpShake();
    }


    void EndPoweredState()
    {
        if (powerSocket == null) return;
        powerSocket.PowerDown();
        powerSocket = null;
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
