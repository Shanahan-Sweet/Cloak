using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    [SerializeField]
    Transform zoomTrans;
    float targetZoom, currentZoom, zoomV;
    [SerializeField] Transform transFollow;
    [SerializeField] float maxMoveSpd = .5f, minMoveSpd = 2, playerBias = 3;
    Vector2 targetPos;

    [SerializeField] List<CamTarget> camTargets = new List<CamTarget>();
    [SerializeField] AnimationCurve targetPullCurve;

    public Camera MainCamera;

    //static reference
    public static MainCam instance;

    void Awake()
    {
        instance = this;
    }
    public void AddToTargetList(CamTarget newTarget)
    {
        camTargets.Add(newTarget);
    }
    public void RemoveFromTargetList(CamTarget newTarget)
    {
        camTargets.Remove(newTarget);
    }

    // Update is called once per frame
    void Update()
    {
        if (transFollow != null)
        {
            float count = playerBias;
            Vector2 meanPos = transFollow.position * playerBias;

            foreach (CamTarget target in camTargets)
            {
                float pDistance = Mathf.InverseLerp(10, 0, (transFollow.position - target.transform.position).magnitude);
                float pull = target.PullStrength * targetPullCurve.Evaluate(pDistance);
                count += pull;
                meanPos += (Vector2)target.transform.position * pull;
            }

            targetPos = meanPos / count;//get mean position
        }

        //if (!isPaused)
        //{
        transform.position = Vector2.Lerp(transform.position, targetPos, Time.unscaledDeltaTime * Mathf.Lerp(minMoveSpd, maxMoveSpd, currentZoom));

        //currentZoom = Mathf.SmoothDamp(currentZoom, targetZoom, ref zoomV, zoomSpd);
        //zoomTrans.localPosition = new Vector3(0, 0, Mathf.Lerp(-4, -13, currentZoom));
        //}
    }

    public void Shake(float time, float strength)
    {
        StartCoroutine(ShakeSequence(time, strength));
    }

    IEnumerator ShakeSequence(float time, float strength)
    {
        float t = Time.unscaledTime + time;
        while (t > Time.unscaledTime)
        {
            //if (!isPaused)
            //{
            transform.position += (Vector3)Random.insideUnitCircle * strength;
            yield return new WaitForSecondsRealtime(.01f);
            //}
            //else//is paused
            //{
            //yield return null;
            //}
        }
    }

    public void SetDefaultZoom(float newDefault)
    {
        targetZoom = newDefault;
    }

}
