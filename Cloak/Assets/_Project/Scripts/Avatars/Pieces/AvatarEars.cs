using UnityEngine;

public class AvatarEars : AvatarGroup
{
    [SerializeField] Transform leftEar, rightEar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public override void AnimUpdate(AvatarValues avatarValues)
    {
        base.AnimUpdate(avatarValues);

    }
}
