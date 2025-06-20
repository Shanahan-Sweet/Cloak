using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    public enum PlayerState
    {
        Move, Jump
    }
    PlayerState currentState;

    [Header("Variables")]
    [SerializeField] float walkSpd = 8;

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

    void FixedUpdate()
    {
        MoveFixed();
    }

    void MoveFixed()
    {

        Vector2 direction = inputScript.MoveAxis;
        direction = platformerScript.CheckGroundedDirection(transform.position, .5f, direction);//don't walk off edges

        rigidBody.AddForce(direction * walkSpd);

        if (!platformerScript.IsAboveGround(transform.position, .05f))
        {
            transform.position = Vector3.zero;
        }
    }

    void JumpFixed()
    {

    }
}
