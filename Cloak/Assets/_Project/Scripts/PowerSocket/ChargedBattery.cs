using System.Collections.Generic;
using UnityEngine;

public class ChargedBattery : IPowerObject
{

    [SerializeField] List<IPowerObject> powerObjects = new List<IPowerObject>();

    int currentCharge = 0;
    [SerializeField] int maxCharge = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public override void SetPowerState(bool powered)
    {
        if (powered) PowerUp();
        else StartLosingPower();//start counting down
    }

    void PowerUp()
    {
        currentCharge = maxCharge;
        CancelInvoke(nameof(LosePower));

        foreach (IPowerObject powerObject in powerObjects)
        {
            powerObject.SetPowerState(true);
        }
    }


    void StartLosingPower()//start counting down
    {
        InvokeRepeating(nameof(LosePower), 5, 5);
    }
    void LosePower()//count down
    {
        currentCharge--;
        if (currentCharge <= 0)
        {
            CancelInvoke(nameof(StartLosingPower));
            PowerDown();
        }
    }
    void PowerDown()//shut down
    {
        foreach (IPowerObject powerObject in powerObjects)
        {
            powerObject.SetPowerState(false);
        }
    }
    /*public override void SetPlayerInput(Vector2 inputDir)
    {

    }*/
}
