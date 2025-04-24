using UnityEngine;

public class PCompanion : MonoBehaviour
{
    [SerializeField] float moveSpd = 20, rotationTorque = 5;
    float targetSide = -1;
    //effects
    [SerializeField] Transform tetherHolder, tetherEnd, midPoints;

    //sprites
    [SerializeField] SpriteRenderer spriteRend;
    [SerializeField] Sprite activeSprite, inactiveSprite;

    //components and references
    CompanionManager playerReference;
    Rigidbody2D rigidBody, playerRigidBody;
    PlatformerPhysics platformerPhysics;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (PlayerMovement.instance)
        {
            InitializeCompanion(PlayerMovement.instance.GetComponent<CompanionManager>());
        }
    }

    void InitializeCompanion(CompanionManager newManager)
    {
        playerReference = newManager;
        playerReference.InitializeCompanion(this);
        platformerPhysics = playerReference.GetComponent<PlatformerPhysics>();
        playerRigidBody = playerReference.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        tetherHolder.rotation = Quaternion.Euler(0, 0, 0);
        if (playerReference == null) return;
        //tether follows player
        tetherEnd.position = playerReference.transform.position;
        tetherEnd.rotation = playerReference.transform.rotation;

        float targetDistance = (playerReference.transform.position - transform.position).magnitude;
        midPoints.localScale = new Vector3(1, Mathf.Clamp01(Mathf.InverseLerp(2, .5f, targetDistance)) + Mathf.Sin(Time.time * .8f) * .08f, 1);
    }

    void FixedUpdate()
    {
        if (playerReference == null) return;
        //get last move direction
        if (Mathf.Abs(playerRigidBody.linearVelocityX) > .75f && (playerReference.transform.position - transform.position).magnitude > 1.3f) targetSide = Mathf.Sign(playerRigidBody.linearVelocityX);

        //calculate target position
        Vector2 targetPos = playerReference.transform.position + new Vector3(0.75f * targetSide, .6f + (platformerPhysics.IsGrounded ? platformerPhysics.GroundDistance * 3 : 0), 0);
        Vector2 pV = playerRigidBody.linearVelocity.magnitude > 2 ? playerRigidBody.linearVelocity.normalized * 2 : playerRigidBody.linearVelocity;
        pV *= .2f;
        targetPos += pV;
        //move to target position
        Vector2 targetDir = targetPos - (Vector2)transform.position;
        targetDir *= .25f;
        if (targetDir.magnitude > 1) targetDir.Normalize();
        rigidBody.AddForce(targetDir * moveSpd);

        //rotate
        FlipUpright(Vector2.up);
    }
    public void FlipUpright(Vector2 pointTarget)
    {
        Quaternion rot = Quaternion.FromToRotation(transform.up, pointTarget);
        rigidBody.AddTorque(rot.z * rotationTorque);
    }
    //dash effects
    public void UseDash()
    {
        spriteRend.sprite = inactiveSprite;
    }

    public void RechargeDash()
    {
        spriteRend.sprite = activeSprite;
    }
}
