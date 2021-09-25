using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BezierManager : MonoBehaviour
{
    [Header("Settings")]
    public float _distanceFromAnchor;
    public float _segmentDistance;
    public bool autoAssignOppositePaths; // i.e. when connecting A to B in the up direction, B will automatically connect to A in the down direction.

    public static float distanceFromAnchor => instance._distanceFromAnchor;
    public static float segmentDistance => instance._segmentDistance;
    public static BezierManager instance;

    void Awake()
    {
        instance = this;
    }

    void OnValidate()
    {
        if(instance == null)
            instance = this;
    }


    ///<Summary>
    /// Anchor Distance is used to not overlap with Bezier anchors (p0, p3) 
    ///</Summary>
    public static void DrawSplineFromPoints(Vector2[] points)
    {
        float arcLength = GetLengthFromPoints(points);
        float segmentStep = segmentDistance / arcLength;
        if(segmentStep <= 0f) return; // to prevent infinite loops.
        
        for(float t = distanceFromAnchor;
                t < 1f - distanceFromAnchor - segmentStep; t += segmentStep)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(GetPositionAtTime(points, t),
                    GetPositionAtTime(points, t + segmentStep));
        }
    }

    ///<Summary>
    /// Based on a 5-line aproximation, reflecting the most complex S-shape
    /// a cubic bezier curve can produce
    ///</Summary>
    public static float GetLengthFromPoints(Vector2[] points)
    {
        const float increment = 0.2f;
        float length = 0f;
        for(float t = 0f; t <= 1f; t += increment)
        {   
            length += Vector2.Distance(GetPositionAtTime(points, t),
                    GetPositionAtTime(points, t + increment));
        }
        return length;
    }

    ///<Summary>
    /// Clamped to 0 <= t <= 1
    ///</Summary>
    public static Vector2 GetPositionAtTime(Vector2[] p, float t)
    {
        t = Mathf.Clamp01(t);
        Vector2 p0 = Mathf.Pow(1 - t, 3) * p[0];
        Vector2 p1 = 3 * Mathf.Pow(1 - t, 2) * t * p[1];
        Vector2 p2 = 3 * (1 - t) * Mathf.Pow(t, 2) * p[2];
        Vector2 p3 = Mathf.Pow(t, 3) * p[3];
        
        return p0 + p1 + p2 + p3;
    }
}
