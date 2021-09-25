using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierNode : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public Transform leftHandle;
    public Transform rightHandle;
    
    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if(start == null || end == null || leftHandle == null || rightHandle == null)
            return;

        BezierManager.DrawSplineFromPoints(GetPoints());
    }

    void OnDrawGizmosSelected()
    {
        if(start == null || end == null || leftHandle == null || rightHandle == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(start.position, leftHandle.position);
        Gizmos.DrawLine(end.position, rightHandle.position);
        Gizmos.DrawIcon(transform.position, "Tex_ts", true);
        Gizmos.DrawIcon(leftHandle.position, "Tex_tn", true);
        Gizmos.DrawIcon(rightHandle.position, "Tex_tn", true);
    }
    #endif

    public Vector2[] GetPoints() => new Vector2[] {
            start.position, leftHandle.position, rightHandle.position, end.position};

    ///<Summary>
    /// For when travelling backwards, P0 and P3 have their positions swapped, and thus P1 and P2 too
    ///</Summary>
    public Vector2[] GetPointsReversed() => new Vector2[] {
            end.position, rightHandle.position, leftHandle.position, start.position};

}
