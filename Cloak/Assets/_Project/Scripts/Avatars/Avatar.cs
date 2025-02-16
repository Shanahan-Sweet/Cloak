using UnityEngine;

public class Avatar : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidBody;
    //[SerializeField] AvatarStance avatarStance;
    [SerializeField] AvatarGroup[] avatarGroups;
    [SerializeField] Transform bobHolder;
    [Header("Variables")]
    [SerializeField] float fastLerpSpd = 10;
    [SerializeField] float slowLerpSpd = 4, velocityLerpSpd = 10, torqueLerpSpd = 20;
    [SerializeField] Vector2 lookTargetPos, moveAxis;
    AvatarValues avatarValues = new AvatarValues();//avatar variables

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public void SetLookPosition(Vector2 newLookTarget)//look at point
    {
        lookTargetPos = new Vector2(newLookTarget.x, newLookTarget.y);
    }

    public void SetInputAxis(Vector2 newAxis)//look at point
    {
        moveAxis = newAxis;
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

        //float bobmagnitude = Mathf.Abs(avatarValues.lerpVelocity.X);
        //bobHolder.Position = new Vector3(0, avatarValues.walkSin1 * .025f * bobmagnitude, 0);//walk bob Y
        //bobHolder.Scale = new Vector3(1 - avatarValues.walkSin1 * .025f * bobmagnitude, 1 + avatarValues.walkSin1 * .025f * bobmagnitude, 1);//walk bob squish
    }
    void UpdateLerps(Vector2 lookDir)
    {
        //Lerp Look Dir
        float t = 1 - Mathf.Pow(0.5f, Time.deltaTime * fastLerpSpd);
        avatarValues.lerpDirFast = Vector2.Lerp(avatarValues.lerpDirFast, moveAxis, t);//quick lerp

        t = 1 - Mathf.Pow(0.5f, Time.deltaTime * slowLerpSpd);
        avatarValues.lerpDirSlow = Vector2.Lerp(avatarValues.lerpDirSlow, moveAxis, t);//slow lerp

        //Velocity
        Vector2 velocity = rigidBody.linearVelocity * .9f;
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
        if (avatarValues.turnDir != dir)//changed direction
        {
            avatarValues.turnDir = dir;

            foreach (AvatarGroup group in avatarGroups)//process animation
            {
                group.SwitchDirection(avatarValues);
            }
        }
    }
}

public class AvatarValues
{
    public Vector2 lerpDirFast, lerpDirSlow;

    public Vector2 lerpVelocity;
    public float lerpTorque;
    //stance
    public float stanceTilt;
    public float turnDir = 0;
    //walk bob
    public float walkSin1, walkCos1, walkSin2, walkCos2;
}