using UnityEngine;

public class RabitLegsAvi : AvatarGroup
{

    [SerializeField] Transform legFlip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    public override void AnimUpdate(AvatarValues avatarValues)
    {
        base.AnimUpdate(avatarValues);


    }

    public override void SwitchDirection(AvatarValues avatarValues)
    {
        legFlip.transform.localScale = new Vector3(avatarValues.turnDir, 1, 1);
    }
}
