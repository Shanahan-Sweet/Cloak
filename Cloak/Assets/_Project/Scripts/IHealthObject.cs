using UnityEngine;

public interface IHealthObject
{
    public void Damage(HitData hitData) { }
    public void Stun(HitData hitData) { }

    public void ExplosionDamage(HitData hitData) { }
}


public class HitData
{
    public int damage;
    public Vector2 pushForce;
}