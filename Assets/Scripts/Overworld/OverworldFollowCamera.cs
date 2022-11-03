using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldFollowCamera : MonoBehaviour
{
    public CameraMode mode;
    public Camera overworldCamera;
    public Transform target;
    public Vector3 targetPosition;
    public Vector2 targetPositionOffset;
    public Vector3 goalPosition;
    public float targetZoom;
    public float levelBoundsZoom;
    public float followSpeed;
    public float zoomSpeed;
    public bool isZoomedOut;

    private Rect levelBounds;
    private Vector3 levelCenter;
    private float defaultZoom;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(levelBounds.xMin, levelBounds.yMin, 0),
                new Vector3(levelBounds.xMax, levelBounds.yMax, 0));
    }

    void Awake()
    {
        defaultZoom = overworldCamera.orthographicSize;
    }

    private void Initialize()
    {
        levelBounds = GetLevelBounds();
        levelCenter = new Vector3(levelBounds.center.x,levelBounds.center.y, transform.position.z);
        levelBoundsZoom = GetOrthographicSizeFromBounds(levelBounds);
    }

    private float GetOrthographicSizeFromBounds(Rect levelBounds)
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = levelBounds.width / levelBounds.height;
 
        if (screenRatio >= targetRatio)
            return levelBounds.height / 2;
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            return levelBounds.height / 2 * differenceInSize;
        }
    }

    private void LateUpdate()
    {
        ZoomUpdate();
        MoveUpdate();
    }

    void MoveUpdate()
    {
        targetPosition = target.position;
        if (transform.position == targetPosition)
            return;
        
        goalPosition = new Vector3 (targetPosition.x, targetPosition.y, transform.position.z);
        goalPosition += (Vector3)targetPositionOffset;
        transform.position = Vector3.Lerp(transform.position, goalPosition, followSpeed);
        
    }

    private void ZoomUpdate()
    {
        if(overworldCamera.orthographicSize == targetZoom)
            return;

        overworldCamera.orthographicSize = Mathf.Lerp(overworldCamera.orthographicSize,
                isZoomedOut ? levelBoundsZoom : targetZoom, zoomSpeed);
        // if(isZoomedOut)
        //     targetPosition = levelCenter;
        // else
        //     targetPosition = player.position;
    }

    public void ToggleZoom()
    {
        isZoomedOut = !isZoomedOut;
    }

    private Rect GetLevelBounds()
    {
        Vector3 min = Vector3.zero;
        Vector3 max = Vector3.zero;
        float tolerance = 4;
        foreach (Transform gridObject in
                FindObjectOfType<Grid>().GetComponentInChildren<Transform>())
        {
            Vector3 position = gridObject.position;
            if (position.x < min.x) min.x = position.x;
            if (position.x > max.x) max.x = position.x;
            if (position.y < min.y) min.y = position.y;
            if (position.y > max.y) max.y = position.y;
            if (position.z < min.z) min.z = position.z;
            if (position.z > max.z) max.z = position.z;
        }

        return Rect.MinMaxRect(min.x - tolerance, min.y - tolerance, max.x + tolerance, max.y + tolerance);
    }

    public void SetCameraMode(CameraMode mode)
    {
        if(mode == CameraMode.FreeRoam)
        {
            targetPositionOffset = Vector2.zero;
            targetZoom = defaultZoom;
        }else if(mode == CameraMode.LevelPreview)
        {
            targetPositionOffset = Vector2.up * 2.5f;
            targetZoom = defaultZoom * 1.2f;
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public enum CameraMode
    {
        FreeRoam,
        LevelPreview
    }
}
