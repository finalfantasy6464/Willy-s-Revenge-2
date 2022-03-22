using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDirections : MonoBehaviour
{
    public PlayerController2021remake playerMovement;
    
    public List<Vector3> directionList;
    Vector3 directionCache;

    void OnDrawGizmos()
    {
        if(directionList == null || directionList.Count < 1)
            return;

        Vector3 lineSum = transform.position - directionList[0];
        Gizmos.DrawLine(transform.position, lineSum);

        if(directionList.Count < 2)
            return;

        for (int i = 0; i < directionList.Count - 1; i++)
        {
            Vector3 lineCache = lineSum - directionList[i + 1];
            Gizmos.DrawLine(lineSum, lineCache);
            lineSum = lineCache;
        }
    }

    void Start()
    {
        directionList = new List<Vector3>();
    }

    void Update()
    {
        if(directionCache != playerMovement.finalMovementVector.normalized)
        {
            directionList.Insert(0, playerMovement.finalMovementVector.normalized);
            directionCache = playerMovement.finalMovementVector.normalized;
            if(directionList.Count > playerMovement.enabledSegmentAmount)
                directionList.RemoveAt(directionList.Count - 1);
        } else if(directionList != null && directionList.Count > 0)
        {
            directionList[0] += playerMovement.finalMovementVector;
            // could change to some other magnitude
            directionList[directionList.Count - 1] -= playerMovement.finalMovementVector;
        }
    }
}
