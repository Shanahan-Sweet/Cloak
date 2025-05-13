using UnityEngine;

public class BreakableWall : BaseHealth
{
    [SerializeField] GameObject spriteHolder;
    [SerializeField] ParticleSystem partDestroy;
    [SerializeField] float partLifetime = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public override void Damage(HitData hitData)
    {
        BreakWall();

    }
    public override void Stun(HitData hitData)
    {

    }

    public override void ExplosionDamage(HitData hitData)
    {
        BreakWall();
    }

    protected virtual void BreakWall()
    {
        if (!isAlive) return;
        isAlive = false;
        Destroy(GetComponent<Collider2D>());//destroy collider
        if (spriteHolder) Destroy(spriteHolder);
        Destroy(this, partLifetime);//delay destroy

        //play effects
        if (partDestroy) partDestroy.Play();
    }
}
