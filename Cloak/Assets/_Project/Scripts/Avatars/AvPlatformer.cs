using UnityEngine;

public class AvPlatformer : AvatarGroup
{
    [SerializeField] PlatformerPhysics platformerPhysics;
    public Transform headHolder, legsHolder, bodyHolder;

    float squish;

    //Rigidbody2D rigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //rigidBody = platformerPhysics.rigidBody;
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
            targetSquish = groundDist / 2;
        }
        else
        {
            targetSquish = avatarValues.lerpVelocity.y * .1f;
        }

        squish = Mathf.MoveTowards(squish, targetSquish, 2f * Time.deltaTime);

        headHolder.localPosition = new Vector3(headHolder.localPosition.x, squish + avatarValues.walkBobSin * .02f, 0);
        legsHolder.localPosition = new Vector3(headHolder.localPosition.x, -squish, 0);

        bodyHolder.localPosition = new Vector3(bodyHolder.localPosition.x, avatarValues.walkBobCos * .02f, 0);
    }
}
