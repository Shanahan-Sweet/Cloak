using UnityEngine;

public class CompanionManager : MonoBehaviour
{
    PCompanion myCompanion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void InitializeCompanion(PCompanion newCompanion)
    {
        myCompanion = newCompanion;
    }

    public void UseDash()
    {
        myCompanion.UseDash();
        CancelInvoke(nameof(RechargeDash));
        Invoke(nameof(RechargeDash), 1.5f);
    }

    void RechargeDash()
    {
        myCompanion.RechargeDash();
    }
}
