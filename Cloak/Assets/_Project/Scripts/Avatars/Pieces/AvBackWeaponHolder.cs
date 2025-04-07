using UnityEngine;

public class AvBackWeaponHolder : AvatarGroup
{
    [SerializeField] Transform holder;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public override void AnimUpdate(AvatarValues avatarValues)
    {
        base.AnimUpdate(avatarValues);


        BackWeaponsAnim(avatarValues);
    }

    void BackWeaponsAnim(AvatarValues avatarValues)
    {
        holder.localPosition = new Vector3(-avatarValues.lerpDirSlow.x * .15f, -avatarValues.lerpVelocity.y * .15f, 0);



        holder.localRotation = Quaternion.Euler(0, 0, -avatarValues.lerpDirSlow.x * 20);
    }
}
