using UnityEngine;

public class RabitLegsAvi : AvatarGroup
{
    [SerializeField] Sprite[] thighSprites;
    [SerializeField] SpriteRenderer leftThigh, rightThigh, leftFoot, rightFoot;
    int myLastTurnDir = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    public override void AnimUpdate(AvatarValues avatarValues)
    {
        base.AnimUpdate(avatarValues);

        SetLegStance(avatarValues);
    }

    public override void SwitchDirection(AvatarValues avatarValues)
    {
        //legFlip.transform.localScale = new Vector3(avatarValues.lastTurnDir, 1, 1);
    }



    void SetLegStance(AvatarValues avatarValues)
    {
        SetLegFlip(avatarValues.lerpDirSlow.x, avatarValues.lastTurnDir);

    }

    void SetLegFlip(float turnDir, float lastTurnDir)
    {
        int turnSet;
        if (Mathf.Abs(turnDir) < .5f)//is facing forward
        {
            turnSet = 0;
        }
        else
        {
            turnSet = (int)Mathf.Sign(turnDir);
        }


        if (myLastTurnDir == turnSet) return;//don't switch

        myLastTurnDir = turnSet;
        if (myLastTurnDir == 0)//set facing forward
        {
            leftThigh.flipX = false;
            rightThigh.flipX = true;
            leftFoot.flipX = lastTurnDir == 1 ? false : true;
            rightFoot.flipX = lastTurnDir == 1 ? false : true;
            SetThighSprite(lastTurnDir == 1 ? thighSprites[1] : thighSprites[0], lastTurnDir == 1 ? thighSprites[0] : thighSprites[1]);
            return;
        }

        if (myLastTurnDir == 1)
        {
            leftThigh.flipX = false;
            rightThigh.flipX = false;
            leftFoot.flipX = false;
            rightFoot.flipX = false;
            SetThighSprite(thighSprites[1], thighSprites[2]);
            return;
        }

        leftThigh.flipX = true;
        rightThigh.flipX = true;
        leftFoot.flipX = true;
        rightFoot.flipX = true;
        SetThighSprite(thighSprites[2], thighSprites[1]);
    }

    void SetThighSprite(Sprite left, Sprite right)
    {
        leftThigh.sprite = left;
        rightThigh.sprite = right;
    }
}
