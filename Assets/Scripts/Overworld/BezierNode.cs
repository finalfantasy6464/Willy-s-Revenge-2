using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierNode : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public Transform leftHandle;
    public Transform rightHandle;
    
    public Vector2[] GetPoints() => new Vector2[] {
            start.position, leftHandle.position, rightHandle.position, end.position};

    ///<Summary>
    /// For when travelling backwards, P0 and P3 have their positions swapped, and thus P1 and P2 too
    ///</Summary>
    public Vector2[] GetPointsReversed() => new Vector2[] {
            end.position, rightHandle.position, leftHandle.position, start.position};

}
