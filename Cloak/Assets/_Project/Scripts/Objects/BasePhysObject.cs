using UnityEngine;

public class BasePhysObject : MonoBehaviour, IKick
{
    [SerializeField] float kickForce = 1;

    Rigidbody2D rigidBody;

    bool stuckToSpike = false;
    Vector2 stuckRotation;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (stuckToSpike) RotateToDirection(transform.up, stuckRotation, 8);//stuck rotation
    }


    //Kick Functions
    public void Kick(Vector2 direction)
    {
        rigidBody.linearVelocity = direction * kickForce;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out KillCollider killCollider))//stick to kill colliders
        {
            if (stuckToSpike) return;//ignore collision
            stuckToSpike = true;

            stuckRotation = transform.up;

            SpringJoint2D spring = gameObject.AddComponent<SpringJoint2D>();
            spring.enableCollision = true;
            spring.autoConfigureConnectedAnchor = false;
            spring.connectedAnchor = transform.position;

            rigidBody.linearDamping = 3;
            rigidBody.angularDamping = 4;

        }
    }

    void RotateToDirection(Vector2 fromDir, Vector2 toDir, float speed)
    {

        rigidBody.AddTorque(Quaternion.FromToRotation(fromDir, toDir).z * speed);//rotate
    }
}
