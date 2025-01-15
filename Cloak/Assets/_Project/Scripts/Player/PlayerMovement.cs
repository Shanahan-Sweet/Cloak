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

    }

    void MoveFixed()
    {
        rigidBody.AddForce(new Vector2(inputScript.MoveAxis.x * moveSpd, 0));

        //add drag
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x * .85f, rigidBody.linearVelocity.y * yDrag);


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
}
