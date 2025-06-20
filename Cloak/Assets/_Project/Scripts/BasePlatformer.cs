using UnityEngine;

public class BasePlatformer : MonoBehaviour
{

    [SerializeField] LayerMask groundMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public Vector2 CheckGroundedDirection(Vector2 startPos, float distance, Vector2 direction)//returns new direction
    {
        direction.Normalize();

        if (!IsAboveGround(startPos + direction * distance, 0.05f))//forwards ground check
        {
            direction = Vector2.zero;//stop moving

        }


        return direction;
    }

    public bool IsAboveGround(Vector2 pos, float radius)
    {
        return Physics2D.OverlapCircle(pos, radius, groundMask);
    }
}
