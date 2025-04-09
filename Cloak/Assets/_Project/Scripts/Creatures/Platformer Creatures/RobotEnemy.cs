using UnityEngine;

public class RobotEnemy : BaseCreature
{
    Vector2 moveAxis = Vector2.zero;
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

        InvokeRepeating(nameof(ChangeWalkDirection), Random.value * 2, Random.Range(.5f, 3f));
    }

    //Update
    private void Update()
    {
        //animation
        myAvatar.SetInputAxis(moveAxis);
    }

    void ChangeWalkDirection()
    {
        moveAxis = new Vector2(Random.Range(-1f, 1f), 0);
        if (Random.value < .15f) Jump();
    }

    //Actions
    public void Jump()
    {
        if (platformerPhysics.TimeFromGrounded > Time.time)
            platformerPhysics.Jump(8);
    }

    void FixedUpdate()
    {
        platformerPhysics.CheckGroundRaycast();//check for ground
        platformerPhysics.CheckCelingRaycast();
        platformerPhysics.ChangeStandHeight(0);

        MoveFixed();
    }

    void MoveFixed()
    {
        // rigidBody.AddForce(new Vector2(inputScript.MoveAxis.x * moveSpd, 0));

        Vector2 moveDir = platformerPhysics.GroundHit ? Quaternion.AngleAxis(-90, Vector3.forward) * platformerPhysics.GroundNormal : Vector2.right;
        rigidBody.AddForce(moveDir * moveAxis * 26);
        //add drag
        //rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x * .85f, rigidBody.linearVelocity.y * yDrag);
        rigidBody.AddForce(new Vector2(-rigidBody.linearVelocity.x * 8, 0));//X drag


        platformerPhysics.KeepWalkHeight();
        platformerPhysics.FlipUpright(Vector2.up);
    }
}
