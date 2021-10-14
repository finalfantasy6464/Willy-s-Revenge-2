using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraFollow : MonoBehaviour
{
	public Camera overworldCamera;

	public Vector3 checkStart;
	public Vector3 checkEnd;
	public Vector3 cameraStart;
	public Vector3 cameraEnd;
	
	public float zoomStart;
	public float zoomEnd;

    private float endCameraHeight;
    private float endCameraWidth;

    private Rect currentRect;
    private Rect endRect;

	public float progressIndex;

    public bool isClamping;

	public OverworldCharacter character;

	[Header("Live Data")]
	public float rawDistance;
	public float clampedDistance;

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    /// 
    public void Update()
    {
        if (isClamping)
        {
            UpdateSize();
            UpdatePosition();
        }
    }

    void UpdateSize()
    {
        float nextSize = Mathf.Lerp(zoomStart, zoomEnd, InverseLerp(checkStart, checkEnd, character.transform.position));
        if (nextSize > overworldCamera.orthographicSize)
            overworldCamera.orthographicSize = nextSize;
    }

    void UpdatePosition()
    {
        Vector3 next = character.transform.position;
        next.z = overworldCamera.transform.position.z;

        float h = 2f * overworldCamera.orthographicSize;
        float w = h * overworldCamera.aspect;

        currentRect = GetCameraRect(next, new Vector2(w, h));
        currentRect.x = Mathf.Clamp(currentRect.x, endRect.xMin, endRect.xMax - currentRect.width);
        currentRect.y = Mathf.Clamp(currentRect.y, endRect.yMin, endRect.yMax - currentRect.height);
        next = new Vector3(currentRect.center.x, currentRect.center.y, next.z);

        overworldCamera.transform.position = next;
    }

    public float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(endRect.center, new Vector3(endRect.width, endRect.height, 1f));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(currentRect.center, new Vector3(currentRect.width, currentRect.height, 1f));

    }
    public void SetCameraState(Vector3 checkStart, Vector3 checkEnd, Vector3 cameraStart, Vector3 cameraEnd, float zoomStart, float zoomEnd)
    {
        this.checkStart = checkStart;
        this.checkEnd = checkEnd;
        this.cameraStart = cameraStart;
        this.cameraEnd = cameraEnd;
        this.zoomStart = zoomStart;
        this.zoomEnd = zoomEnd;
        endCameraHeight = 2f * zoomEnd;
        endCameraWidth = endCameraHeight * overworldCamera.aspect;
        endRect = GetCameraRect(cameraEnd, new Vector2(endCameraWidth, endCameraHeight));
    }

    Rect GetCameraRect(Vector2 center, Vector2 size)
    {
        return new Rect(new Vector2(center.x - (size.x / 2f), center.y - (size.y / 2f)), size);
    }
}
