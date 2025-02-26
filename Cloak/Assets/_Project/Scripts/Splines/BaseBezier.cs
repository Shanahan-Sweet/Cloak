using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBezier : MonoBehaviour
{
    public static Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float ttt = tt * t;
        float uu = u * u;
        float uuu = uu * u;

        Vector3 p = (uuu * p0) + (3 * uu * t * p1) + (3 * u * tt * p2) + (ttt * p3);

        return p;
    }
}
