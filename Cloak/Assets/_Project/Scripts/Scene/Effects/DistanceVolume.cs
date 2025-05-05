using UnityEngine;

public class DistanceVolume : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public float sourceVolume = 1;
    [SerializeField] float minDist = 1.5f, maxDist = 5;
    [SerializeField] bool randomPitch = true;
    float startPitch = 1;


    bool isActive = false;
    float currentVolume, targetVolume;
    Transform playerTrans;


    // Start is called before the first frame update
    void Start()
    {
        playerTrans = PlayerMovement.instance.transform;
        currentVolume = 0;
        audioSource.volume = currentVolume;

        if (randomPitch)
        {
            startPitch = Random.Range(.75f, 1.25f);
            audioSource.pitch = startPitch;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTrans == null) return;

        float distance = (transform.position - playerTrans.position).magnitude;
        targetVolume = Mathf.InverseLerp(maxDist, minDist, distance);

        float changeSpd = 4;
        float t = Time.deltaTime * changeSpd;
        currentVolume = Mathf.MoveTowards(currentVolume, targetVolume, t);

        if (currentVolume > .01f)
        {
            if (isActive == false)
            {
                isActive = true;
                audioSource.Play();
            }
            audioSource.volume = currentVolume * sourceVolume;
        }
        else
        {
            if (isActive == true)
            {
                isActive = false;
                audioSource.Stop();
            }
        }
    }

    void OnEnable()
    {
        isActive = false;
        audioSource.Stop();
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()//display selected chunk
    {
        Gizmos.color = new Color(.25f, 0.25f, 1, .07f);
        Vector2 pos = (Vector2)transform.position;
        Gizmos.DrawSphere(pos, maxDist);

        Gizmos.color = new Color(1, 0.25f, .5f, 0.15f);
        Gizmos.DrawSphere(pos, minDist);
    }
#endif
}
