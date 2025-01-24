using UnityEngine;

public class PlayerMovement : MonoBehaviour
{    //state
    public enum PlayerState
    {
        Move,
        Stunned,
        Dead,
    }
    public PlayerState currentState;

    //variables
    [SerializeField] float moveSpd = 10, jumpHeight = 7, rotationTorque = 20;

    float yDrag = 1;

    //Components
    PlayerInput inputScript;

    Rigidbody2D rigidBody;

    void Awake()
    {
        inputScript = GetComponent<PlayerInput>();
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

        Vector2 moveDir = inputScript.GroundHit ? Quaternion.AngleAxis(-90, Vector3.forward) * inputScript.GroundNormal : Vector2.right;
        rigidBody.AddForce(moveDir * inputScript.MoveAxis.x * moveSpd);

        //add drag
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x * .85f, rigidBody.linearVelocity.y * yDrag);

        KeepWalkHeight();
        FlipUpright(Vector2.up);
    }

    //Actions
    public void MoveJump()
    {
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpHeight);
        //effects
        //anim.SetTrigger("Jump");
        //AudioManager.instance.PlaySound3D(jumpSnd, 5, .2f, transform.position, Random.Range(.9f, 1.1f));
    }

    //Physics stuff
    void FlipUpright(Vector2 pointTarget)
    {
        Quaternion rot = Quaternion.FromToRotation(transform.up, pointTarget);
        rigidBody.AddTorque(rot.z * rotationTorque);
    }

    void KeepWalkHeight()
    {
        if (inputScript.IsGrounded && inputScript.GroundHit)
        {
            rigidBody.AddForce(new Vector2(0, -rigidBody.linearVelocity.y * 10));//Y drag

            Vector2 targetPos = inputScript.GroundPoint + Vector2.up * .7f;
            rigidBody.AddForce(new Vector2(0, (targetPos - (Vector2)transform.position).y) * 60);


        }
    }
}
