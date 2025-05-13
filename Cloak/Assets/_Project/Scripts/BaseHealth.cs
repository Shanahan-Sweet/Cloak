using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public virtual void Damage(HitData hitData) { }
    public virtual void Stun(HitData hitData) { }

    public virtual void ExplosionDamage(HitData hitData) { }

    protected bool isAlive = true;
}


public class HitData
{
    public int damage;
    public Vector2 pushForce;
}
