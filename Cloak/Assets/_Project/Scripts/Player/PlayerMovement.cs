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
    [SerializeField] float moveSpd = 25, jumpHeight = 7;
    [SerializeField] float slideSpd = 30;

    //float yDrag = 1;

    //Components
    PlayerInput inputScript;
    PlatformerPhysics platformerPhysics;
    Rigidbody2D rigidBody;

    Vector2 GroundNormal { get { return Quaternion.AngleAxis(-90, Vector3.forward) * platformerPhysics.GroundNormal; } }

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
        // rigidBody.AddForce(new Vector2(inputScript.MoveAxis.x * moveSpd, 0));

        if (platformerPhysics.GroundHit && inputScript.MoveAxis.y < -.25f)//holding down
        {
            Vector2 moveDir = GroundNormal * -Mathf.Sign(GroundNormal.y);//get down direction
            rigidBody.AddForce(moveDir * slideSpd);
            directionTrans.position = (Vector2)transform.position + moveDir;//debug
        }
        else
        {
            Vector2 moveDir = platformerPhysics.GroundHit ? GroundNormal : Vector2.right;
            rigidBody.AddForce(moveDir * inputScript.MoveAxis.x * moveSpd);
            directionTrans.position = (Vector2)transform.position + moveDir;//debug
        }
        //add drag
        //rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x * .85f, rigidBody.linearVelocity.y * yDrag);
        rigidBody.AddForce(new Vector2(-rigidBody.linearVelocity.x * 8, 0));//X drag


        platformerPhysics.KeepWalkHeight();
        platformerPhysics.FlipUpright(Vector2.up);
    }

    //Actions
    public void Jump()
    {
        //float tempHeight = currentState == PlayerState.Attack ? jumpHeight * .5f : jumpHeight;
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

        Vector2 moveDir = platformerPhysics.GroundHit ? Quaternion.AngleAxis(-90, Vector3.forward) * platformerPhysics.GroundNormal : Vector2.right;
        rigidBody.AddForce(moveDir * inputScript.MoveAxis.x * moveSpd * .1f);
        directionTrans.position = (Vector2)transform.position + moveDir;
        //add drag
        rigidBody.AddForce(new Vector2(-rigidBody.linearVelocity.x * (platformerPhysics.IsGrounded ? 8 : .5f), 0));//X drag


        platformerPhysics.KeepWalkHeight();
        platformerPhysics.FlipUpright(Vector2.up);
    }
}
