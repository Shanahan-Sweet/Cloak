using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySpline : MonoBehaviour
{
    bool initialized = false;
    [SerializeField] LineRenderer[] lineRend;
    [SerializeField] List<Transform> transPoints = new List<Transform>();
    private int totalPointCount;
    [SerializeField] int segmentPointCount = 10;
    private Vector3[] linePositions;

    [SerializeField] float angleSmoothness = 2;

    // Start is called before the first frame update
    void Start()
    {
        if (initialized == false)
        {
            SetPointCount(segmentPointCount);
            //endHolder.parent = null;


            ResetPositions();
        }
    }

    public void Initialize(Transform startTrans, List<Transform> transforms)
    {
        initialized = true;

        transPoints = transforms;
        transPoints.Insert(0, startTrans);

        segmentPointCount = transforms.Count;
        SetPointCount(segmentPointCount);

        ResetPositions();
    }

    public void SetPointCount(int newCount)
    {
        segmentPointCount = newCount;

        totalPointCount = segmentPointCount * (transPoints.Count - 1);

        linePositions = new Vector3[totalPointCount];
        foreach (LineRenderer temp in lineRend)
        {
            temp.positionCount = totalPointCount;
        }
    }
    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        DrawCurve();
    }

    protected void DrawCurve()
    {

        for (int i = 0; i < transPoints.Count - 1; i++)//each segment
        {
            for (int j = 0; j < segmentPointCount; j++)//each point
            {
                float t = j / (float)(segmentPointCount - 1);// * 1.1f;
                linePositions[j + (i * segmentPointCount)] = BaseBezier.CalculateCubicBezierPoint(t, transPoints[i].position, transPoints[i].position - transPoints[i].right * angleSmoothness, transPoints[i + 1].position + transPoints[i + 1].right * angleSmoothness, transPoints[i + 1].position);
                //linePositions[i] = BaseBezier.CalculateCubicBezierPoint(t, transPoints[0].position, transPoints[1].position, transPoints[2].position, transPoints[3].position) + AddToLine(t);
            }
        }

        foreach (LineRenderer temp in lineRend)
        {
            temp.SetPositions(linePositions);
        }
    }

    public void ResetPositions()
    {
        DrawCurve();
    }
}
