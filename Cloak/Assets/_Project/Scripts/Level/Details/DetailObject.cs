using UnityEngine;

public class DetailObject : MonoBehaviour
{
    [SerializeField] GameObject destroyObject;
    [SerializeField] ParticleSystem partDestroy;
    [SerializeField] float partLife;



    protected bool destroyed = false;


    //audio
    [SerializeField] AudioClip destroySnd;

    //add to cam list
    protected virtual void Start()
    {

    }

    //public voids
    public virtual void DestroyMe()
    {
        if (destroyed == false)
        {
            destroyed = true;


            //effects
            if (partDestroy)
                partDestroy.Play();

            Destroy(destroyObject);
            Destroy(gameObject, partLife);

            //destroy collider
            Collider2D myCol = GetComponent<Collider2D>();
            if (myCol != null)
            {
                Destroy(myCol);
            }

            //audio
            // AudioManager.instance.PlaySfxSound(AudioManager.AudioChanel.Detail, destroySnd, transform.position, 30, Random.Range(.7f, 1.3f));

        }
    }

    public virtual void Collide(float speed)
    {

    }
}
