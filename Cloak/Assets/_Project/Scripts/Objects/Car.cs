using UnityEngine;

public class Car : IPowerObject
{
    bool isPowered = false;
    Vector2 playerInputDir;

    Rigidbody2D rigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (!isPowered) return;

        float rotDir = playerInputDir.x * -4;
        float moveSpd = playerInputDir.y * 15;

        rigidBody.AddTorque(rotDir);//rotate
        rigidBody.AddForce(transform.right * moveSpd);


        /*if (isPowered && playerInputDir.magnitude > .25f)
        {
            RotateToDirection(transform.right, playerInputDir.normalized, 8);
            float dirForce = Mathf.Clamp01(Vector2.Dot(transform.right, playerInputDir.normalized));
            rigidBody.AddForce(transform.right * 15 * dirForce);
        }*/
    }
    void RotateToDirection(Vector2 fromDir, Vector2 toDir, float speed)
    {

        rigidBody.AddTorque(Quaternion.FromToRotation(fromDir, toDir).z * speed);//rotate
    }
    public override void SetPowerState(bool powered)
    {
        if (powered == false)
        {
            isPowered = false;
            playerInputDir = Vector2.zero;
        }
        else
        {
            isPowered = true;
            playerInputDir = Vector2.zero;
        }
    }

    public override void SetPlayerInput(Vector2 inputDir)
    {
        playerInputDir = inputDir;
    }
}
