using UnityEngine;

public class AvatarCloak : AvatarGroup
{
    PlatformerPhysics platformerPhysics;
    [SerializeField] Transform holder, leftBone, rightBone;
    float squish;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platformerPhysics = GetComponentInParent<Avatar>().platformerPhysics;
    }


    public override void AnimUpdate(AvatarValues avatarValues)
    {
        base.AnimUpdate(avatarValues);

        AnimateCloak(avatarValues);

    }

    void AnimateCloak(AvatarValues avatarValues)
    {
        float targetSquish;
        if (avatarValues.isGrounded)
        {
            float groundDist = platformerPhysics.GroundDistance;
            targetSquish = groundDist * 2;
        }
        else
        {
            targetSquish = 0;
        }
        squish = Mathf.MoveTowards(squish, targetSquish, 2f * Time.deltaTime);


        float yCloakSpread = avatarValues.lerpVelocity.y + squish;
        float xRot = -avatarValues.lerpVelocity.x * 20;
        float yRot = yCloakSpread * 15;
        leftBone.localRotation = Quaternion.Euler(0, 0, xRot + yRot);
        rightBone.localRotation = Quaternion.Euler(0, 0, xRot - yRot);
    }
}
