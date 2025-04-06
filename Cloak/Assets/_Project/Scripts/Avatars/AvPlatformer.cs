using UnityEngine;

public class AvPlatformer : AvatarGroup
{
    public Transform headHolder, legsHolder;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public override void AnimUpdate(AvatarValues avatarValues)
    {
        base.AnimUpdate(avatarValues);


        BodySquishAnim(avatarValues);
    }

    void BodySquishAnim(AvatarValues avatarValues)
    {
        float look = avatarValues.lerpDirFast.y * .05f;
        float squish = avatarValues.isGrounded ? 0 : avatarValues.lerpVelocity.y * .1f;

        headHolder.localPosition = new Vector3(headHolder.localPosition.x, squish + look, 0);
        legsHolder.localPosition = new Vector3(headHolder.localPosition.x, -squish, 0);
    }
}
