using UnityEngine;

public class Avatar : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] public PlatformerPhysics platformerPhysics;
    //[SerializeField] AvatarStance avatarStance;
    [SerializeField] AvatarGroup[] avatarGroups;
    [SerializeField] Transform bobHolder;
    [Header("Variables")]
    [SerializeField] float fastLerpSpd = 10;
    [SerializeField] float slowLerpSpd = 4, velocityLerpSpd = 10, torqueLerpSpd = 20;
    [SerializeField] float walkSinSpeed = 16;
    [SerializeField] Vector2 lookTargetPos, moveAxis;
    AvatarValues avatarValues = new AvatarValues();//avatar variables

    Animator anim;

    // Awake
    void Awake()
    {
        anim = GetComponent<Animator>();

        avatarValues.sinSpd = walkSinSpeed;//set sin wave speed
    }
    public void SetLookPosition(Vector2 newLookTarget)//look at point
    {
        lookTargetPos = new Vector2(newLookTarget.x, newLookTarget.y);
    }

    public void SetInputAxis(Vector2 newAxis)//look at point
    {
        moveAxis = newAxis;
    }

    public void UpdateGroundedState(bool value)
    {
        avatarValues.isGrounded = value;
    }

    //Actions
    public virtual void Jump()
    {
        if (anim != null) anim.SetTrigger("Jump");
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 look = lookTargetPos - (Vector2)transform.position;//look towards point
        if (look.magnitude > 1)
        {
            look = look.normalized;
        }

        UpdateLerps(look);
        //UpdateBob();
        UpdateDirection(look);

        foreach (AvatarGroup group in avatarGroups)//process animation
        {
            group.AnimUpdate(avatarValues);
        }
        avatarValues.walkBobSin = Mathf.Lerp(0, Mathf.Sin(Time.time * avatarValues.sinSpd) * avatarValues.lerpVelocity.x, avatarValues.groundedT);
        avatarValues.walkBobCos = Mathf.Lerp(0, Mathf.Cos(Time.time * avatarValues.sinSpd) * avatarValues.lerpVelocity.x, avatarValues.groundedT);
        //float bobmagnitude = Mathf.Abs(avatarValues.lerpVelocity.X);
        //bobHolder.Position = new Vector3(0, avatarValues.walkSin1 * .025f * bobmagnitude, 0);//walk bob Y
        //bobHolder.Scale = new Vector3(1 - avatarValues.walkSin1 * .025f * bobmagnitude, 1 + avatarValues.walkSin1 * .025f * bobmagnitude, 1);//walk bob squish
    }
    void UpdateLerps(Vector2 lookDir)
    {
        avatarValues.groundedT = Mathf.MoveTowards(avatarValues.groundedT, avatarValues.isGrounded ? 1 : 0, 1.5f * Time.deltaTime);

        //Lerp Look Dir
        float t = 1 - Mathf.Pow(0.5f, Time.deltaTime * fastLerpSpd);
        avatarValues.lerpDirFast = Vector2.Lerp(avatarValues.lerpDirFast, moveAxis, t);//quick lerp

        t = 1 - Mathf.Pow(0.5f, Time.deltaTime * slowLerpSpd);
        avatarValues.lerpDirSlow = Vector2.Lerp(avatarValues.lerpDirSlow, moveAxis, t);//slow lerp

        //Velocity
        Vector2 velocity = rigidBody.linearVelocity * .5f;
        if (velocity.magnitude > 1) velocity = velocity.normalized;//normalize velocity value
        t = 1 - Mathf.Pow(0.5f, Time.deltaTime * velocityLerpSpd);
        avatarValues.lerpVelocity = Vector2.Lerp(avatarValues.lerpVelocity, velocity, t);//velocity lerp

        //Torque
        t = 1 - Mathf.Pow(0.5f, Time.deltaTime * torqueLerpSpd);
        avatarValues.lerpTorque = Mathf.Lerp(avatarValues.lerpTorque, -rigidBody.angularVelocity, t);//slow lerp
    }

    void UpdateDirection(Vector2 lookDir)
    {
        float dir = Mathf.Sign(avatarValues.lerpDirSlow.x);
        if (avatarValues.lastTurnDir != dir)//changed direction
        {
            avatarValues.lastTurnDir = dir;

            foreach (AvatarGroup group in avatarGroups)//process animation
            {
                group.SwitchDirection(avatarValues);
            }
        }
    }
}

public class AvatarValues
{
    public bool isGrounded = true;
    public float groundedT;
    public Vector2 lerpDirFast, lerpDirSlow;

    public Vector2 lerpVelocity;
    public float lerpTorque;

    //stance
    public float stanceTilt;
    public float lastTurnDir = 1;
    //walk bob
    public float sinSpd;
    public float walkBobSin, walkBobCos;
}