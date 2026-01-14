using UnityEngine;
using System.Collections.Generic;
public class WallDetection : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameObject detectionVisualHolder;
    [SerializeField] Transform closestPointTrans, jumpDirectionTrans;



    bool isColliding = false;
    int currentCollisionCount;
    public int CurrentCollisionCount { get { return currentCollisionCount; } }
    public Vector2 JumpDir { get { return (transform.position - closestPointTrans.position).normalized; } }

    List<Collider2D> collisions = new List<Collider2D>();

    IKick kickObj;
    public IKick KickObj { get { return kickObj; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void LateUpdate()
    {
        GetClosestPoint();
    }

    void GetClosestPoint()
    {
        kickObj = null;//reset kick object

        if (collisions.Count == 0 && detectionVisualHolder)
        {
            detectionVisualHolder.SetActive(false);
            return;
        }

        Vector2 point = closestPointTrans.position;
        float dist = 100;

        foreach (Collider2D col in collisions)
        {
            Vector2 p = col.ClosestPoint(transform.position);

            float d = (p - (Vector2)transform.position).magnitude;
            if (d < dist)
            {
                point = p;
                dist = d;

                //kick object
                if (col.TryGetComponent(out IKick otherKick))
                {
                    kickObj = otherKick;
                }
            }
        }
        if (detectionVisualHolder) detectionVisualHolder.SetActive(true);
        closestPointTrans.position = point;
        jumpDirectionTrans.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, JumpDir));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        collisions.Add(collision);
        currentCollisionCount++;
        ChangeCollisionCount();
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        collisions.Remove(collision);
        currentCollisionCount--;
        ChangeCollisionCount();
    }

    void ChangeCollisionCount()
    {
        if (isColliding && currentCollisionCount == 0)
        {
            isColliding = false;
            playerMovement.UpdateWallDetection(isColliding);
        }
        else if (!isColliding && currentCollisionCount != 0)
        {
            isColliding = true;
            playerMovement.UpdateWallDetection(isColliding);
        }
    }
}
