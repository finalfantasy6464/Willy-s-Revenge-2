using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonToggle : MonoBehaviour
{
    public Camera mainCamera;
    public float zoomLevel;
    public Vector3 cameraPosition;

    public ColourWaypoints[] togglers;

    public void WaypointBehaviour()
    {
        {
            mainCamera.transform.position = cameraPosition;
            mainCamera.orthographicSize = zoomLevel;
            mainCamera.GetComponent<CameraFollow>().isClamping = false;
            mainCamera.backgroundColor = Color.black;

            foreach (ColourWaypoints toggle in togglers)
            {
                toggle.gameObject.SetActive(false);
            }
        }
    }
}
