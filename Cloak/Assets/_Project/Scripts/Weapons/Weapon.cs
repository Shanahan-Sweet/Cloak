using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform weaponRotation;
    [SerializeField] WeaponCollider weaponCollider;
    //components
    IWeaponMaster myWeaponMaster;
    [SerializeField] Animator weaponAnim;

    // Awake
    void Awake()
    {
        myWeaponMaster = GetComponentInParent<IWeaponMaster>();
        weaponCollider.gameObject.SetActive(false);
    }

    public void StartAttack(Vector2 pointDirection)
    {
        //Reset attack
        CancelInvoke(nameof(AttackStrike));
        DisableAttackCollider();
        CancelInvoke(nameof(DisableAttackCollider));
        CancelInvoke(nameof(EndAttack));
        weaponAnim.ResetTrigger("Strike");


        //start attack
        //rotate to target direction
        float rotZ = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
        Quaternion newAngle = Quaternion.AngleAxis(rotZ, Vector3.forward);
        float t = 1 - Mathf.Pow(0.5f, Time.deltaTime * 3f);//2.6f
        weaponRotation.rotation = newAngle;

        //start attack
        Invoke(nameof(AttackStrike), .5f);//wind up time
        weaponAnim.SetTrigger("SwordReady");
    }

    void AttackStrike()
    {
        Invoke(nameof(EndAttack), .25f);//release time
        weaponAnim.SetTrigger("Strike");

        myWeaponMaster.AttackStrike(weaponRotation.right);

        weaponCollider.gameObject.SetActive(true);//hit collider

        Invoke(nameof(DisableAttackCollider), .5f);//disable collider
    }

    void EndAttack()
    {
        myWeaponMaster.AttackEnded();
    }


    void DisableAttackCollider()
    {
        weaponCollider.gameObject.SetActive(false);
    }
}
