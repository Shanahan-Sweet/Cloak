using UnityEngine;

public class HealthRBody : MonoBehaviour, IHealthObject
{
    //variables
    [Header("Rigidbody")]
    [SerializeField] Vector2 hitReactForce = Vector2.one;
    [SerializeField] float hitUpForce = .5f;
    //components
    protected Rigidbody2D rigidBody;
    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {

    }

    public void Damage(HitData hitData)
    {
        print("hit");

        Stun(hitData);
    }
    public void Stun(HitData hitData)
    {
        ApplyPushForce(hitData);
    }

    public void ExplosionDamage(HitData hitData)
    {
        ApplyPushForce(hitData);
    }
    void ApplyPushForce(HitData hitData)
    {
        rigidBody.linearVelocity = (hitData.pushForce + new Vector2(0, hitUpForce)) * hitReactForce;
    }
}
