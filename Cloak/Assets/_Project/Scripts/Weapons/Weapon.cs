using UnityEngine;

public class Weapon : MonoBehaviour
{

    //components
    IWeaponMaster myWeaponMaster;
    [SerializeField] Animator weaponAnim;

    // Awake
    void Awake()
    {
        myWeaponMaster = GetComponentInParent<IWeaponMaster>();
    }

    public void StartAttack()
    {
        //Reset attack
        CancelInvoke(nameof(AttackStrike));
        CancelInvoke(nameof(EndAttack));
        weaponAnim.ResetTrigger("Strike");

        //start attack
        Invoke(nameof(AttackStrike), .5f);//wind up time
        weaponAnim.SetTrigger("SwordReady");
    }

    void AttackStrike()
    {
        Invoke(nameof(EndAttack), .25f);//release time
        weaponAnim.SetTrigger("Strike");

        myWeaponMaster.AttackStrike();
    }

    void EndAttack()
    {
        myWeaponMaster.AttackEnded();
    }
}
