using UnityEngine;

public class AvPlatformer : AvatarGroup
{
    PlatformerPhysics platformerPhysics;
    public Transform headHolder, legsHolder, bodyHolder;

    float squish;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platformerPhysics = GetComponentInParent<Avatar>().platformerPhysics;
    }
    public override void AnimUpdate(AvatarValues avatarValues)
    {
        base.AnimUpdate(avatarValues);


        BodySquishAnim(avatarValues);
    }

    void BodySquishAnim(AvatarValues avatarValues)
    {

        float targetSquish;
        if (avatarValues.isGrounded)
        {
            float groundDist = platformerPhysics.GroundDistance;
            targetSquish = Mathf.Clamp(groundDist, -.1f, 0.1f);
            targetSquish += -Mathf.Abs(avatarValues.lerpVelocity.x) * .1f;
        }
        else
        {
            targetSquish = avatarValues.lerpVelocity.y * .1f;
        }

        squish = Mathf.MoveTowards(squish, targetSquish, 2f * Time.deltaTime);

        headHolder.localPosition = new Vector3(headHolder.localPosition.x, squish * .5f + avatarValues.walkBobSin * .02f, 0);
        legsHolder.localPosition = new Vector3(headHolder.localPosition.x, -squish, 0);

        bodyHolder.localPosition = new Vector3(bodyHolder.localPosition.x, avatarValues.walkBobCos * .02f, 0);
    }
}
