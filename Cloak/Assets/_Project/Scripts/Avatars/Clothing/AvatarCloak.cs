using UnityEngine;

public class AvatarCloak : AvatarGroup
{
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

    }
}
