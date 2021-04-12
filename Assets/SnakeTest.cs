using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTest : MonoBehaviour
{
    public Transform head;
    public Transform[] segments;
    List<Vector3> breadcrumbs;

    public float segmentSpacing; //set controls the spacing between the segments,which is always constant.

    void Start()
    {
        //populate the first set of crumbs by the initial positions of the segments.
        breadcrumbs = new List<Vector3>();
        breadcrumbs.Add(head.position); //add head first, because that's where the segments will be going.
        for (int i = 0; i < segments.Length; i++) // we have an extra-crumb to mark where the last segment was...
            breadcrumbs.Add(segments[i].position);
    }

    void Update()
    {

        float headDisplacement = (head.position - breadcrumbs[0]).magnitude;

        if (headDisplacement >= segmentSpacing)
        {
            breadcrumbs.RemoveAt(breadcrumbs.Count - 1); //remove the last breadcrumb
            breadcrumbs.Insert(0, head.position); // add a new one where head is.
            headDisplacement = headDisplacement % segmentSpacing;
        }

        if (headDisplacement != 0)
        {
            Vector3 pos = Vector3.Lerp(breadcrumbs[1], breadcrumbs[0], headDisplacement / segmentSpacing);
            segments[0].position = pos;

            for (int i = 1; i < segments.Length; i++)
            {
                pos = Vector3.Lerp(breadcrumbs[i + 1], breadcrumbs[i], headDisplacement / segmentSpacing);
                segments[i].position = pos;
            }
        }

    }
}
