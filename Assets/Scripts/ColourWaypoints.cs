using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourWaypoints : MonoBehaviour
{
    public Transform checkStart;
    public Transform checkEnd;

    public Vector3 cameraStart;
    public Vector3 cameraEnd;

    public Vector3 HemisphereView;
    public Vector3 FullWorldView;
    public Vector3 FullCloudView;

    public bool resetsCamera;

    public bool isBackwards;

    public float zoomStart;
    public float zoomEnd;

    public int progressIndex;

    public int viewSetting;

    public OverworldCharacter character;

    public BackgroundColourController bgController;

    public CameraFollow camerafollow;

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            WaypointBehaviour();
        }
    }

    public void WaypointBehaviour()
    {
        if (isBackwards)
        {
            camerafollow.isClamping = false;
            if (viewSetting == 0)
            {
                camerafollow.overworldCamera.transform.position = HemisphereView;
                camerafollow.overworldCamera.orthographicSize = zoomEnd;
            }
            else if (viewSetting == 1)
            {
                camerafollow.transform.position = FullWorldView;
                camerafollow.overworldCamera.orthographicSize = zoomEnd;
            }
            else if (viewSetting == 2)
            {
                camerafollow.transform.position = FullCloudView;
                camerafollow.overworldCamera.orthographicSize = zoomEnd;
            }
            return;
        }
        camerafollow.isClamping = true;

        bgController.progressIndex = progressIndex;
        bgController.SetVectorTransforms(checkStart, checkEnd);

        camerafollow.progressIndex = progressIndex;
        camerafollow.SetCameraState(checkStart.position, checkEnd.position, cameraStart, cameraEnd, zoomStart, zoomEnd);

        if (resetsCamera)
        {
            camerafollow.overworldCamera.orthographicSize = zoomStart;
        }
    }
}
