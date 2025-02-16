using UnityEngine;

public class AvatarGroup : MonoBehaviour
{
    [SerializeField] AvatarPiece[] moveTransforms, slowTransforms;

    public virtual void AnimUpdate(AvatarValues avatarValues)
    {
        foreach (AvatarPiece piece in moveTransforms)
        {
            piece.SetPos(avatarValues.lerpDirFast, avatarValues.lerpTorque);
        }
        foreach (AvatarPiece piece in slowTransforms)
        {
            piece.SetPos(avatarValues.lerpDirSlow, avatarValues.lerpTorque);
        }
    }

    public virtual void SwitchDirection(AvatarValues avatarValues)
    {

    }
}
