using UnityEngine;

public class CompanionManager : MonoBehaviour
{
    PCompanion myCompanion;
    PlatformerPhysics platformerPhysics;
    // Awake
    void Awake()
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

    //animation
    void Update()
    {

    }
}
