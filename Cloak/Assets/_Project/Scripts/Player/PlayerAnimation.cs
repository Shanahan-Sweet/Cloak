using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Transform headTrans, tailTrans, facePosition, faceTrans;
    [SerializeField] SpriteRenderer[] eyeRend;
    [SerializeField] Sprite eyesDefault, eyesCharge;

    float rot, targetRot, rotV;

    float twistAngle = 45;


    [SerializeField] Transform pointDirection;

    //Animations
    bool shake = false;
    [SerializeField] Transform shakeHolder;
    float shakeMagnitude = .02f, shakeInterval = .05f, shakeT;

    //Particles
    [SerializeField] ParticleSystem partGrub, partGrubSmall, partChargeJump;

    PlayerInput inputScript;
    Rigidbody2D rigidBody;
    [SerializeField] Animator anim;

    void Awake()
    {
        inputScript = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        float r = Vector2.Dot(transform.right, inputScript.GetRawAxis);

        targetRot = -r * twistAngle;

        rot = Mathf.SmoothDamp(rot, targetRot, ref rotV, .2f);


        headTrans.localRotation = Quaternion.Euler(0, 0, rot);
        tailTrans.localRotation = Quaternion.Euler(0, 0, 180 - rot);


        //face position
        faceTrans.position = facePosition.position;

        //point direction
        pointDirection.rotation = Quaternion.FromToRotation(new Vector2(-inputScript.GetRawAxis.x, inputScript.GetRawAxis.y), Vector2.up);

        //Shake
        ShakeSprite();
    }

    public void SetShake(bool value)
    {
        if (value == shake) return;
        bool oldValue = shake;
        shake = value;

        SetEyeSprite(shake ? eyesCharge : eyesDefault);//temp eye state

        if (oldValue == true && value == false)//switch off
        {
            shakeHolder.localPosition = Vector2.zero;
            return;
        }
    }

    void ShakeSprite()
    {
        if (shake == false || shakeT > Time.time) return;
        shakeT = Time.time + shakeInterval;
        shakeHolder.localPosition = Random.insideUnitCircle * shakeMagnitude;
    }

    //States
    public void SetMove()
    {
        twistAngle = 45;
    }

    public void SetClimb()
    {
        twistAngle = 15;
    }
    public void SetStun()
    {
        twistAngle = 45;
        anim.SetBool("Charging", false);
        anim.SetBool("ChargedJump", false);
        anim.SetBool("Boosting", false);
    }

    public void StartChargingJump()
    {
        anim.SetBool("Charging", true);
        anim.SetBool("ChargedJump", false);
    }

    public void ChargeJump()
    {
        anim.SetBool("Charging", false);
        anim.SetBool("ChargedJump", true);
        partChargeJump.Play();
    }

    public void ReleaseJump(bool boost)
    {
        anim.SetBool("Charging", false);
        anim.SetBool("ChargedJump", false);
        anim.SetBool("Boosting", boost);

        if (boost) PlayGrubParticles();
    }
    public void EndBoost()
    {
        anim.SetBool("Boosting", false);
    }
    //Particles and effects
    public void PlayGrubParticles()
    {
        partGrub.Play();
    }
    public void PlaySmallGrubParticles()
    {
        partGrubSmall.Play();
    }




    void SetEyeSprite(Sprite sprite)
    {
        eyeRend[0].sprite = sprite;
        eyeRend[1].sprite = sprite;
    }
}
