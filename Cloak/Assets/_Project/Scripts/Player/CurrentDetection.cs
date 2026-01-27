using System.Collections.Generic;
using UnityEngine;

public class CurrentDetection : MonoBehaviour
{
    [SerializeField] Collider2D myCollider;
    List<Collider2D> waterCurrents = new List<Collider2D>();

    bool collideWithCurrents = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeCollisionState(bool allowCollisions)
    {
        if (collideWithCurrents == allowCollisions) return;//no change
        collideWithCurrents = allowCollisions;

        print("waterCurrents:" + waterCurrents.Count);

        if (collideWithCurrents)//enable collisions
        {
            foreach (Collider2D col in waterCurrents)
            {
                Physics2D.IgnoreCollision(col, myCollider, false);
            }
            return;
        }

        //ignore collisions
        foreach (Collider2D col in waterCurrents)
        {
            Physics2D.IgnoreCollision(col, myCollider, true);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        waterCurrents.Add(collision);
        if (!collideWithCurrents)
        {
            Physics2D.IgnoreCollision(collision, myCollider, true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        waterCurrents.Remove(collision);

        if (!collideWithCurrents)
        {
            Physics2D.IgnoreCollision(collision, myCollider, false);
        }
    }
}
