using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourWaypoints : MonoBehaviour
{
    public Transform checkStart;
    public Transform checkEnd;

    public Vector3 cameraStart;
    public Vector3 cameraEnd;

    public float zoomStart;
    public float zoomEnd;

    public int progressIndex;

    public OverworldCharacter character;

    public BackgroundColourController bgController;

    public CameraFollow camerafollow;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            bgController.progressIndex = progressIndex;
            bgController.SetVectorTransforms(checkStart, checkEnd);

            camerafollow.progressIndex = progressIndex;
            camerafollow.SetCameraState(checkStart.position, checkEnd.position, cameraStart, cameraEnd, zoomStart, zoomEnd);
        }
    }
}
