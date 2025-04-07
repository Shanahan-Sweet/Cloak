using UnityEngine;

public class AvPlatformer : AvatarGroup
{
    [SerializeField] PlatformerPhysics platformerPhysics;
    public Transform headHolder, legsHolder;

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
        float look = avatarValues.lerpDirFast.y * .05f;
        float targetSquish;
        if (avatarValues.isGrounded)
        {
            float groundDist = platformerPhysics.GroundDistance;
            targetSquish = groundDist;
        }
        else
        {
            targetSquish = avatarValues.lerpVelocity.y * .1f;
        }

        squish = Mathf.MoveTowards(squish, targetSquish, 2f * Time.deltaTime);

        headHolder.localPosition = new Vector3(headHolder.localPosition.x, squish + look, 0);
        legsHolder.localPosition = new Vector3(headHolder.localPosition.x, -squish, 0);
    }
}
