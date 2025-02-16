using UnityEngine;

public class AvatarStance : AvatarGroup
{
    [SerializeField] Transform topStanceRotation, bottomStanceRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public override void AnimUpdate(AvatarValues avatarValues)
    {
        base.AnimUpdate(avatarValues);

        StanceAnim(avatarValues);
    }

    protected virtual void StanceAnim(AvatarValues avatarValues)
    {
        topStanceRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, avatarValues.lerpVelocity.x * -15));
    }
}
