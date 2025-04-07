using UnityEngine;

public class AvatarCloak : AvatarGroup
{

    [SerializeField] Transform leftBone, rightBone;

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
        leftBone.localRotation = Quaternion.Euler(0, 0, -avatarValues.lerpVelocity.x * 20);
        rightBone.localRotation = Quaternion.Euler(0, 0, -avatarValues.lerpVelocity.x * 20);
    }
}
