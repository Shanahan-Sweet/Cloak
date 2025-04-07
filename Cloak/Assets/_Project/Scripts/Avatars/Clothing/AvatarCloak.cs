using UnityEngine;

public class AvatarCloak : AvatarGroup
{

    [SerializeField] Transform holder, leftBone, rightBone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    public override void AnimUpdate(AvatarValues avatarValues)
    {
        base.AnimUpdate(avatarValues);

        AnimateCloak(avatarValues);

    }

    void AnimateCloak(AvatarValues avatarValues)
    {
        float xRot = -avatarValues.lerpVelocity.x * 20;
        float yRot = avatarValues.lerpVelocity.y * 15;
        leftBone.localRotation = Quaternion.Euler(0, 0, xRot + yRot);
        rightBone.localRotation = Quaternion.Euler(0, 0, xRot - yRot);
    }
}
