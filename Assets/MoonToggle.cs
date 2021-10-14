using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonToggle : MonoBehaviour
{
    public Camera mainCamera;
    public float zoomLevel;
    public Vector3 cameraPosition;

    public ColourWaypoints[] togglers;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.CompareTag("Player"))
        {
            mainCamera.transform.position = cameraPosition;
            mainCamera.orthographicSize = zoomLevel;
            mainCamera.GetComponent<CameraFollow>().isClamping = false;

            foreach (ColourWaypoints toggle in togglers)
            {
                toggle.gameObject.SetActive(false);
            }
        }
    }
}
