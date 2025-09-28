using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour
{

    Vector2 rawAxis;
    public Vector2 GetRawAxis { get { return rawAxis; } }
    float timeFromJumpInput, timeFromLastJump;
    PlayerMovement moveScript;
    PlayerAnimation animScript;



    void Awake()
    {
        moveScript = GetComponent<PlayerMovement>();
        animScript = GetComponent<PlayerAnimation>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rawAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (rawAxis.magnitude > .8) rawAxis.Normalize();

        if (Input.GetButtonDown("Jump"))
        {
            //timeFromJumpInput = Time.time + .2f;

            PressJump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            timeFromJumpInput = Time.time + .2f;
        }
        if (timeFromJumpInput > Time.time && timeFromLastJump < Time.time)
        {
            ReleaseJump();
        }
    }

    void PressJump()
    {
        moveScript.PressJump();
    }


    void ReleaseJump()
    {
        timeFromLastJump = Time.time + .2f;
        moveScript.ReleaseJump();
    }
}