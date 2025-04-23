using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleQuadBezier : SimpleBezierLine
{
    protected override void DrawCurve()
    {
        for (int i = 0; i < pointCount; i++)
        {
            float t = i / ((float)pointCount - 1);// * 1.1f;
            linePositions[i] = BaseBezier.CalculateQuadraticBezierPoint(t, transPoints[0].position, transPoints[1].position, transPoints[2].position) + AddToLine(t);
        }

        foreach (LineRenderer temp in lineRend)
        {
            temp.SetPositions(linePositions);
        }
    }
}
