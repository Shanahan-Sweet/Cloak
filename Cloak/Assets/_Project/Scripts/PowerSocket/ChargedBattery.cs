using System.Collections.Generic;
using UnityEngine;

public class ChargedBattery : IPowerObject
{

    [SerializeField] List<IPowerObject> powerObjects = new List<IPowerObject>();
    [SerializeField] List<PowerLevelDisplay> powerLevelDisplay = new List<PowerLevelDisplay>();
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

        foreach (PowerLevelDisplay powerDisplay in powerLevelDisplay)//update power level displays
        {
            powerDisplay.UpdatePowerLevel(1);

        }
    }


    void StartLosingPower()//start counting down
    {
        InvokeRepeating(nameof(LosePower), 1, 1);
    }
    void LosePower()//count down
    {
        currentCharge--;
        float powerPercent = (float)currentCharge / maxCharge;
        print(powerPercent);
        foreach (PowerLevelDisplay powerDisplay in powerLevelDisplay)//update power level displays
        {
            powerDisplay.UpdatePowerLevel(powerPercent);
        }

        if (currentCharge <= 0)
        {
            CancelInvoke(nameof(LosePower));
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
