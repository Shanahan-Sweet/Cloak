using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PoweredLight : IPowerObject
{
    float currentPower = 0;
    float targetPower = 0, chargeSpd = 1;

    [SerializeField] Light2D myLight;
    [SerializeField] AnimationCurve brightnessCurve;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetRandomChargeSpd();

        UpdateLight();

        Invoke(nameof(Flicker), Random.Range(.1f, 20));
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPower == targetPower) return;

        currentPower = Mathf.MoveTowards(currentPower, targetPower, Time.deltaTime * chargeSpd);

        UpdateLight();
    }

    void UpdateLight()
    {
        myLight.intensity = brightnessCurve.Evaluate(currentPower);
    }

    void Flicker()
    {
        Invoke(nameof(Flicker), Random.Range(.1f, 20));
        if (currentPower == 0) return;
        SetRandomChargeSpd();

        currentPower -= Random.value;
    }
    void SetRandomChargeSpd()
    {
        chargeSpd = Random.Range(.1f, 1);
    }



    public override void SetPowerState(bool powered)
    {
        SetRandomChargeSpd();
        if (powered)
        {
            targetPower = 1;
        }
        else
        {
            targetPower = 0;
        }
    }

    public override void UpdatePowerLevel(float powerLevel)
    {
        Flicker();
    }
}
