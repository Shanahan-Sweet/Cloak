using UnityEngine;

public class SimpleLegs : AvatarGroup
{    //legs
    [SerializeField] Transform legTransformHolder;
    [SerializeField] Transform[] legHolder, legPos;
    [SerializeField] float stepMagnitude, stepSpd = 7.5f;
    int stepCheck = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public override void AnimUpdate(AvatarValues avatarValues)
    {
        base.AnimUpdate(avatarValues);
        AnimateLegs(avatarValues);

    }

    void AnimateLegs(AvatarValues avatarValues)
    {
        Vector2[] footTargetPos = new Vector2[2];
        Vector2[] legTargetPos = new Vector2[2];

        legTransformHolder.localRotation = Quaternion.Euler(0, 0, -avatarValues.lerpVelocity.x * 20);
        //legTransformHolder.localPosition = new Vector3(-avatarValues.lerpVelocity.x * 0.05f, 0, 0);
        if (avatarValues.isGrounded)
        {
            footTargetPos[0] = avatarValues.lerpVelocity.x * new Vector2(Mathf.Sin(Time.time * stepSpd * avatarValues.lastTurnDir), Mathf.Cos(Time.time * stepSpd * avatarValues.lastTurnDir)) * stepMagnitude;
            footTargetPos[1] = avatarValues.lerpVelocity.x * new Vector2(Mathf.Sin(Time.time * stepSpd * avatarValues.lastTurnDir + 3), Mathf.Cos(Time.time * stepSpd * avatarValues.lastTurnDir + 3)) * stepMagnitude;

            legTargetPos[0] = new Vector2(-.1f - Mathf.Max(-.1f, -Mathf.Abs(avatarValues.lerpVelocity.x * .2f)), 0);
            legTargetPos[1] = new Vector2(.1f + Mathf.Max(-.1f, -Mathf.Abs(avatarValues.lerpVelocity.x * .2f)), 0);

            //step snd
            if (legHolder[stepCheck].localPosition.y < -.02f)
            {
                stepCheck = stepCheck == 1 ? 0 : 1;//switch
                //audio
                //AudioManager.instance.PlaySound3D(stepSnd, 57, .2f, transform.position, Random.Range(.8f, 1.2f));
            }
        }
        else
        {
            legTargetPos[0] = new Vector2(-.1f, 0);
            legTargetPos[1] = new Vector2(.1f, 0);
        }

        legHolder[0].localPosition = Vector2.Lerp(legHolder[0].localPosition, footTargetPos[0], Time.deltaTime * 15);
        legHolder[1].localPosition = Vector2.Lerp(legHolder[1].localPosition, footTargetPos[1], Time.deltaTime * 15);

        legPos[0].localPosition = Vector2.Lerp(legPos[0].localPosition, legTargetPos[0], Time.deltaTime * 15);
        legPos[1].localPosition = Vector2.Lerp(legPos[1].localPosition, legTargetPos[1], Time.deltaTime * 15);
    }
}
