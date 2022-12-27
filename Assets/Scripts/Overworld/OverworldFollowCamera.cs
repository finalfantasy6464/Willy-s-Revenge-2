using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldFollowCamera : MonoBehaviour
{
    public CameraMode mode;
    public Camera overworldCamera;
    public Transform target;
    public Vector3 goalPosition;
    public Vector3 targetPosition;
    public Vector2 targetPositionOffset;
    public float targetZoom;
    public float levelBoundsZoom;
    public float followSpeed;
    public float zoomSpeed;
    public bool isZoomedOut;
    [Space]
    public Vector2 freeRoamPositionOffset;
    public float freeRoamTargetZoom;
    public float freeRoamTargetZoomCache;
    public Vector2 levelPreviewPositionOffset;
    public float levelPreviewTargetZoom;
    public Vector2 gatePreviewPositionOffset;
    public float gatePreviewTargetZoom;

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

    private void Start()
    {
        if(GameControl.control.savedCameraPosition != null)
        transform.position = GameControl.control.savedCameraPosition;
        if (freeRoamTargetZoomCache == 0)
        {
            freeRoamTargetZoomCache = freeRoamTargetZoom;
        }
            levelPreviewTargetZoom = freeRoamTargetZoomCache + 1.5f;
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

    private void FixedUpdate()
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

    public void SetCameraMode(CameraMode mode)
    {
        if (mode == CameraMode.FreeRoam)
        {
            targetPositionOffset = freeRoamPositionOffset;
            targetZoom = freeRoamTargetZoomCache;
        }
        else if (mode == CameraMode.LevelPreview)
        {
            if (targetZoom < 3.5f)
            {
                targetPositionOffset = levelPreviewPositionOffset - new Vector2(0, 2);
            }
            else if (targetZoom == 3.5f)
            {
                targetPositionOffset = levelPreviewPositionOffset - new Vector2(0, 1);
            }
            else
            {
                targetPositionOffset = levelPreviewPositionOffset;
            }
            targetZoom = Mathf.Min(6.2f, levelPreviewTargetZoom);
        }
        else if (mode == CameraMode.GatePreview)
        {
            targetPositionOffset = gatePreviewPositionOffset;
            targetZoom = gatePreviewTargetZoom;
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
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

    public void SetCameraInstant(Vector3 position, Vector3 goalPosition, float zoom = -1f)
    {
        if(targetZoom != -1)
            targetZoom = zoom;

        overworldCamera.orthographicSize = zoom;
        transform.position = position;
        this.goalPosition = goalPosition;
    }

    public enum CameraMode
    {
        FreeRoam,
        LevelPreview,
        GatePreview
    }
}
