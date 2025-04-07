using UnityEngine;

public class AvatarEars : AvatarGroup
{
    [SerializeField] AvatarPiece leftEar, rightEar;
    [SerializeField] Vector2 earPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        earPos = leftEar.transform.localPosition;
    }

    public override void AnimUpdate(AvatarValues avatarValues)
    {
        base.AnimUpdate(avatarValues);

        float xPos = 1 - Mathf.Abs(avatarValues.lerpDirFast.x);
        Vector2 setPos = new Vector2(Mathf.Lerp(earPos.x * .7f, earPos.x, xPos), earPos.y);

        //move ears
        leftEar.transform.localPosition = new Vector3(setPos.x, setPos.y, 0);
        rightEar.transform.localPosition = new Vector3(-setPos.x, setPos.y, 0);
        //rotate ears
        leftEar.SetRotation(avatarValues.lerpDirFast, avatarValues.lerpTorque);
        rightEar.SetRotation(avatarValues.lerpDirFast, avatarValues.lerpTorque);
    }
}
