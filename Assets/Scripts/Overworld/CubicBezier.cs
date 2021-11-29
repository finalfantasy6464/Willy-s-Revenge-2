using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<Summary>
/// The cubic Bézier curve can be defined as an affine combination of two quadratic Bézier curves:
/// P0 = ((1 - t)^3) * cP0
/// P1 = ((3(1 - t)^2) * t) * cP1
/// P2 = ((3(1 - t)) * t^2) * cP2
/// P3 = (t^3) * cP3
/// B(t) = P0 + P1 + P2 + P3
/// while 0 <= t <= 1
///</Summary>
public class CubicBezier : MonoBehaviour
{
    public Transform[] points;
    [Header("Live Data")]
    public float arcLength;

    Vector2 gizmoPosition;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        DrawSpline();
        DrawControlPolygon();
    }

    void DrawControlPolygon()
    {
        Gizmos.color = (Color.gray + Color.white) / 2f;
        Gizmos.DrawLine(points[1].position, points[2].position);
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(points[0].position, points[1].position);
        Gizmos.DrawLine(points[3].position, points[2].position);
    }

    ///<Summary>
    /// Anchor Distance is used to not overlap with Bezier anchors (p0, p3) 
    ///</Summary>
    void DrawSpline()
    {
        arcLength = GetLengthFromPoints(points);
        float segmentStep = BezierManager.segmentDistance / arcLength;
        
        if(segmentStep <= 0f) return; // to prevent infinite loops.
        
        for(float t = BezierManager.distanceFromAnchor;
                t < 1f - BezierManager.distanceFromAnchor; t += segmentStep)
        {
            Gizmos.DrawIcon(GetPositionFromTime(t), "Tex_ts");
        }
    }

    ///<Summary>
    /// From 0 to 1. Returns Vector2.zero otherwise
    ///</Summary>
    public Vector2 GetPositionFromTime(float t)
    {
        Vector2 p0 = Mathf.Pow(1 - t, 3) * points[0].position;
        Vector2 p1 = 3 * Mathf.Pow(1 - t, 2) * t * points[1].position;
        Vector2 p2 = 3 * (1 - t) * Mathf.Pow(t, 2) * points[2].position;
        Vector2 p3 = Mathf.Pow(t, 3) * points[3].position;
        
        return p0 + p1 + p2 + p3;
    }

    ///<Summary>
    /// Calculated based on a 5-point aproximation, representing the angles
    /// of an "S"-shape; the most complex a cubic bezier curve can produce.
    ///</Summary>
    public float GetLengthFromPoints(Transform[] pointObjects)
    {
        return GetLengthFromPoints(
                pointObjects[0].position, pointObjects[1].position,
                pointObjects[2].position, pointObjects[3].position);
    }

    public float GetLengthFromPoints(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        const float increment = 0.2f;
        float length = 0f;
        for(float t = 0f; t <= 1f; t += increment)
        {   
            length += Vector2.Distance(GetPositionFromTime(t), GetPositionFromTime(t + increment));
        }
        return length;
    }
}
