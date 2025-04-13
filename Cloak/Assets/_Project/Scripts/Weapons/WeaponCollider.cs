using UnityEngine;

public class WeaponCollider : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IHealthObject otherHealth))
        {
            HitData hitData = new HitData()
            {
                damage = 2,
                pushForce = transform.right * 4,
            };
            otherHealth.Damage(hitData);
        }
    }
}
