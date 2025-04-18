using UnityEngine;

public class PlatformerPhysics : MonoBehaviour
{

    //Variables
    [SerializeField]
    float standHeight = .8f, rotationTorque = 16, legSeparation = .13f;
    float StandHeight { get { return standHeight + standHAdd + worldStandAdd; } }
    [SerializeField] float groundRayAddGrounded = .2f, groundRayAddAirborne = -.15f, standHeightForce = 60f;
    float standHAdd = 0, worldStandAdd = 0;
    float defaultGravity;
    //Timers
    float timeFromGrounded, timeFromJump;
    public float TimeFromGrounded { get { return timeFromGrounded; } }

    //check for ground
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    public bool IsGrounded { get { return isGrounded && groundHit.groundHit; } }

    public float GroundDistance { get { return transform.position.y - GroundPoint.y - standHeight; } }

    //ground raycasts
    GroundPoint groundHit = new GroundPoint();
    public bool GroundHit { get { return groundHit.groundHit; } }
    public GroundPoint GrounPointInfo { get { return groundHit; } }
    public Vector2 GroundPoint { get { return groundHit.groundPoint; } }
    public Vector2 GroundNormal { get { return groundHit.groundNormal; } }
    [SerializeField] Transform[] groundPointTransform;//debug
    //ground raycasts
    GroundPoint cellingHit = new GroundPoint();
    //Components
    public Rigidbody2D rigidBody;

    [SerializeField] Avatar myAvatar;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        defaultGravity = rigidBody.gravityScale;
    }


    public void ChangeStandHeight(float addValue)
    {
        standHAdd = addValue;
    }

    //Actions
    public void Jump(float jumpHeight)
    {
        timeFromGrounded = 0;
        timeFromJump = Time.time + .25f;

        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpHeight);
        //effects
        myAvatar.Jump();
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

        Vector2 targetPos = GroundPoint + Vector2.up * StandHeight;
        rigidBody.AddForce(new Vector2(0, (targetPos - (Vector2)transform.position).y) * standHeightForce);
    }
    //Ground Check

    public void CheckGroundRaycast()
    {
        float groundCheckDist = standHeight + (isGrounded ? groundRayAddGrounded : groundRayAddAirborne);
        GroundPoint middleHit = DownRay((Vector2)transform.position, groundCheckDist, Vector2.down);//center
        GroundPoint leftHit = DownRay((Vector2)transform.position - Vector2.right * legSeparation, groundCheckDist, Vector2.down);
        GroundPoint rightHit = DownRay((Vector2)transform.position + Vector2.right * legSeparation, groundCheckDist, Vector2.down);

        groundHit.groundHit = leftHit.groundHit && rightHit.groundHit;

        if (!leftHit.groundHit)
            groundHit = middleHit.groundHit ? middleHit : rightHit;
        else if (!rightHit.groundHit)
            groundHit = middleHit.groundHit ? middleHit : leftHit;
        else
        {
            groundHit.groundPoint = (leftHit.groundPoint + rightHit.groundPoint) / 2;
            groundHit.groundNormal = (leftHit.groundNormal + rightHit.groundNormal) / 2;
            groundHit.groundNormal.Normalize();
        }

        //debug
        if (groundPointTransform.Length >= 2)
        {
            groundPointTransform[0].position = leftHit.groundPoint;
            groundPointTransform[1].position = groundHit.groundPoint;
            groundPointTransform[2].position = rightHit.groundPoint;
        }
        CheckForGround();
    }
    void CheckForGround()
    {
        //bool atGroundLevel = ((Vector2)transform.position - groundHit.groundPoint).y < StandHeight * 1f;
        if (timeFromJump < Time.time && rigidBody.linearVelocity.y < 4 && groundHit.groundHit)// && atGroundLevel)
        {
            timeFromGrounded = Time.time + .15f;
            if (!isGrounded)
            {
                isGrounded = true;
                rigidBody.gravityScale = 0;
                myAvatar.UpdateGroundedState(isGrounded);//anim
                //effects
                //partLand.Play();
            }
        }
        else if (isGrounded)//set airborne
        {
            isGrounded = false;
            rigidBody.gravityScale = defaultGravity;
            myAvatar.UpdateGroundedState(isGrounded);//anim
        }
    }

    public void CheckCelingRaycast()
    {

        float groundCheckDist = standHeight * 2.2f;
        cellingHit = DownRay(groundHit.groundPoint + new Vector2(0, .2f), groundCheckDist, Vector2.up);//center

        groundPointTransform[3].position = cellingHit.groundPoint;


        if (isGrounded && cellingHit.groundHit)
        {
            float celingDist = Mathf.Clamp01(Mathf.InverseLerp(groundCheckDist, standHeight * 1.7f, cellingHit.groundPoint.y - groundHit.groundPoint.y));
            worldStandAdd = -celingDist * .175f;
        }
        else worldStandAdd = 0;
    }

    GroundPoint DownRay(Vector2 origin, float length, Vector2 direction)
    {
        GroundPoint hitData = new GroundPoint();
        hitData.groundHit = false;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, length, groundMask);
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
