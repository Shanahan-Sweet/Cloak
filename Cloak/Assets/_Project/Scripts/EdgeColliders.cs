using UnityEngine;

public class EdgeColliders : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] bool isVisible = false;
    [SerializeField] GameObject[] colliderRenderer;
    [Header("Variables")]
    [SerializeField] Transform followTransform;
    [SerializeField] Transform[] checkHolder, colliderTransform;
    [SerializeField] float farDist = 1.2f, nearDist = .8f;
    [SerializeField] LayerMask groundMask;
    float checkRadius = .1f;
    [Header("ColliderState")]
    [SerializeField] GameObject activeHolder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject spr in colliderRenderer)
        {
            spr.SetActive(isVisible);
        }
    }

    public void SetCollidersActive(bool setActive)
    {
        activeHolder.SetActive(setActive);
    }


    void FixedUpdate()//update collider positions
    {
        for (int i = 0; i < checkHolder.Length; i++)
        {
            MoveCollider(i);
        }
    }

    void MoveCollider(int index)
    {
        Vector2 checkPos = followTransform.position + checkHolder[index].position + checkHolder[index].transform.right * farDist;
        if (!IsAboveGround(checkPos))//point is above air
        {
            RaycastHit2D hit = Physics2D.Raycast(checkPos, -checkHolder[index].transform.right, farDist, groundMask);
            if (hit.collider != null)//hit
            {
                float colTargetDist = Mathf.Lerp(0, farDist, Mathf.InverseLerp(farDist, 0, hit.distance));
                colliderTransform[index].position = followTransform.position + checkHolder[index].transform.right * (colTargetDist + nearDist);
                return;
            }

        }
        //above ground
        colliderTransform[index].position = followTransform.position + checkHolder[index].transform.right * (farDist + nearDist);
    }

    bool IsAboveGround(Vector2 pos)
    {
        return Physics2D.OverlapCircle(pos, checkRadius, groundMask);
    }
}
