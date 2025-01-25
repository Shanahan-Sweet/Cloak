using UnityEngine;

public class PlatformerPhysics : MonoBehaviour
{

    //Variables
    [SerializeField]
    float standHeight = .8f, rotationTorque = 16;

    //Timers
    float timeFromGrounded, timeFromJump;
    public float TimeFromGrounded { get { return timeFromGrounded; } }

    //check for ground
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    public bool IsGrounded { get { return isGrounded && groundHit.groundHit; } }

    //ground raycasts
    GroundPoint groundHit = new GroundPoint();
    public bool GroundHit { get { return groundHit.groundHit; } }
    public Vector2 GroundPoint { get { return groundHit.groundPoint; } }
    public Vector2 GroundNormal { get { return groundHit.groundNormal; } }
    [SerializeField] Transform[] groundPointTransform;//debug

    //Components
    Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }


    //Actions
    public void Jump(float jumpHeight)
    {
        timeFromGrounded = 0;
        timeFromJump = Time.time + .25f;

        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpHeight);
        //effects
        //anim.SetTrigger("Jump");
        //AudioManager.instance.PlaySound3D(jumpSnd, 5, .2f, transform.position, Random.Range(.9f, 1.1f));
    }

    //Physics stuff
    public void FlipUpright(Vector2 pointTarget)
    {
        Quaternion rot = Quaternion.FromToRotation(transform.up, pointTarget);
        rigidBody.AddTorque(rot.z * rotationTorque);
    }

    public void KeepWalkHeight()
    {
        if (!isGrounded || !GroundHit) return;

        rigidBody.AddForce(new Vector2(0, -rigidBody.linearVelocity.y * 10));//Y drag

        Vector2 targetPos = GroundPoint + Vector2.up * standHeight;
        rigidBody.AddForce(new Vector2(0, (targetPos - (Vector2)transform.position).y) * 60);
    }
    //Ground Check
    public void CheckForGround()
    {
        if (timeFromJump < Time.time && rigidBody.linearVelocity.y < 4 && Physics2D.OverlapCircle(transform.position + new Vector3(0, -.6f, 0), .1f, groundMask))
        {
            timeFromGrounded = Time.time + .15f;
            if (!isGrounded)
            {
                isGrounded = true;
                //effects
                //partLand.Play();
            }
        }
        else if (isGrounded)//set airborne
        {
            isGrounded = false;
        }
    }

    public void CheckGroundRaycast()
    {
        float groundCheckDist = .9f;
        GroundPoint leftHit = DownRay((Vector2)transform.position - Vector2.right * .13f, groundCheckDist);
        GroundPoint rightHit = DownRay((Vector2)transform.position + Vector2.right * .13f, groundCheckDist);

        groundHit.groundHit = leftHit.groundHit && rightHit.groundHit;

        if (!leftHit.groundHit)
            groundHit = rightHit;
        else if (!rightHit.groundHit)
            groundHit = leftHit;
        else
        {
            groundHit.groundPoint = (leftHit.groundPoint + rightHit.groundPoint) / 2;
            groundHit.groundNormal = (leftHit.groundNormal + rightHit.groundNormal) / 2;
            groundHit.groundNormal.Normalize();
        }


        groundPointTransform[0].position = leftHit.groundPoint;
        groundPointTransform[1].position = groundHit.groundPoint;
        groundPointTransform[2].position = rightHit.groundPoint;
    }

    GroundPoint DownRay(Vector2 origin, float length)
    {
        GroundPoint hitData = new GroundPoint();
        hitData.groundHit = false;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, length, groundMask);
        if (hit.collider)
        {
            hitData.groundHit = true;
            hitData.groundPoint = hit.point;
            hitData.groundNormal = hit.normal;
        }

        return hitData;
    }
}

public class GroundPoint
{
    public bool groundHit;
    public Vector2 groundPoint;
    public Vector2 groundNormal;
}
