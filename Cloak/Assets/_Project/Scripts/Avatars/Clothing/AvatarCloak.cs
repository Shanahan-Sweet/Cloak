using UnityEngine;

public class AvatarCloak : AvatarGroup
{
    PlatformerPhysics platformerPhysics;
    [SerializeField] Transform holder, leftBone, rightBone;
    [SerializeField] float squish, directionT;
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
        directionT = Mathf.MoveTowards(directionT, avatarValues.lastTurnDir, 4f * Time.deltaTime);
        float dir = (directionT + 1) / 2;//set 0 to 1
        float bounceSin1 = Mathf.Sin(Time.time * avatarValues.sinSpd * .75f) * 8;//slow
        float bounceSin2 = Mathf.Sin(Time.time * avatarValues.sinSpd * 2) * 3;//fast bounce

        float walkT = avatarValues.groundedT * Mathf.Abs(avatarValues.lerpVelocity.x);
        float bounceLeft = Mathf.LerpUnclamped(0, Mathf.LerpUnclamped(bounceSin2, bounceSin1, dir), walkT);//bounce
        float bounceRight = Mathf.LerpUnclamped(0, Mathf.LerpUnclamped(bounceSin1, bounceSin2, dir), walkT);//bounce

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
        leftBone.localRotation = Quaternion.Euler(0, 0, xRot + yRot + bounceLeft);
        rightBone.localRotation = Quaternion.Euler(0, 0, xRot - yRot + bounceRight);

        transform.localPosition = new Vector3(avatarValues.lerpDirSlow.x * -.02f, transform.localPosition.y, 0);
    }
}
