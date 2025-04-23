using UnityEngine;

public class PCompanion : MonoBehaviour
{


    [SerializeField] float moveSpd = 20;

    //effects
    [SerializeField] Transform tetherEnd, midPoints;

    //sprites
    [SerializeField] SpriteRenderer spriteRend;
    [SerializeField] Sprite activeSprite, inactiveSprite;

    //components and references
    CompanionManager playerReference;
    Rigidbody2D rigidBody;
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
    }

    // Update is called once per frame
    void Update()
    {
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
        Vector2 targetPos = playerReference.transform.position + new Vector3(-.75f, 1, 0);
        rigidBody.AddForce((targetPos - (Vector2)transform.position) * moveSpd);
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
