using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBezierLine : MonoBehaviour
{
    public LineRenderer[] lineRend;
    public Transform[] transPoints;
    public int pointCount = 15;
    public Vector3[] linePositions;

    // Start is called before the first frame update
    void Awake()
    {
        SetPointCount(pointCount);
        //endHolder.parent = null;


        ResetPositions();
    }

    public virtual void SetPointCount(int newCount)
    {
        pointCount = newCount;

        linePositions = new Vector3[pointCount];
        foreach (LineRenderer temp in lineRend)
        {
            temp.positionCount = pointCount;
        }
    }

    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        DrawCurve();
    }

    protected virtual void DrawCurve()
    {
        for (int i = 0; i < pointCount; i++)
        {
            float t = i / (float)pointCount;// * 1.1f;
            linePositions[i] = BaseBezier.CalculateCubicBezierPoint(t, transPoints[0].position, transPoints[1].position, transPoints[2].position, transPoints[3].position) + AddToLine(t);
        }

        foreach (LineRenderer temp in lineRend)
        {
            temp.SetPositions(linePositions);
        }
    }


    protected virtual Vector3 AddToLine(float t)
    {
        return Vector3.zero;
    }

    //base functions
    public void ResetPositions()
    {

        DrawCurve();
    }
}
