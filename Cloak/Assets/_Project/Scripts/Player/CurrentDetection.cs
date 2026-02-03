using System.Collections.Generic;
using UnityEngine;

public class CurrentDetection : MonoBehaviour
{
    List<WaterCurrent> waterCurrents = new List<WaterCurrent>();

    bool collideWithCurrents = true;

    float currentForceMultiplier = 1;

    Rigidbody2D rigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Fixed Update
    void FixedUpdate()
    {
        if (currentForceMultiplier == 0) return;

        Vector2 forceDir = Vector2.zero;
        float maxForce = 0;
        foreach (WaterCurrent col in waterCurrents)
        {
            forceDir += col.CurrentVelocity;
            maxForce = Mathf.Max(maxForce, col.CurrentVelocity.magnitude);
        }

        if (forceDir.magnitude > maxForce)
        {
            forceDir.Normalize();
            forceDir *= maxForce;
        }

        if (forceDir.magnitude == 0) return;//no force

        rigidBody.AddForce(forceDir * currentForceMultiplier);//add current force
    }

    public void ChangeCollisionState(bool allowCollisions)
    {
        if (collideWithCurrents == allowCollisions) return;//no change
        collideWithCurrents = allowCollisions;

        currentForceMultiplier = allowCollisions ? 1 : 0;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out WaterCurrent current)) return;
        waterCurrents.Add(current);


    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out WaterCurrent current)) return;
        waterCurrents.Remove(current);

    }
}
