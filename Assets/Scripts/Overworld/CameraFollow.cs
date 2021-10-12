using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Camera overworldCamera;

	public Vector3 checkStart;
	public Vector3 checkEnd;
	public Vector3 cameraStart;
	public Vector3 cameraEnd;
	
	public float zoomStart;
	public float zoomEnd;

	public float progressIndex;

	public OverworldCharacter character;

	[Header("Live Data")]
	public float rawDistance;
	public float clampedDistance;

	/// <summary>
	/// Update is called once per frame
	/// </summary>
	public void Update ()
	{
		UpdatePosition();
		UpdateSize();
	}

    void UpdateSize()
	{ 
		float nextSize = Mathf.Lerp(zoomStart, zoomEnd, InverseLerp(checkStart, checkEnd, character.transform.position));
		if (nextSize > overworldCamera.orthographicSize)
        {
			overworldCamera.orthographicSize = nextSize;
		}
	}

    void UpdatePosition()
    {
		overworldCamera.transform.position = Vector3.Lerp(cameraStart, cameraEnd, InverseLerp(checkStart, checkEnd, character.transform.position));
    }

	public float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
	{
		Vector3 AB = b - a;
		Vector3 AV = value - a;
		return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
	}

	public void SetCameraState(Vector3 checkStart, Vector3 checkEnd, Vector3 cameraStart, Vector3 cameraEnd, float zoomStart, float zoomEnd)
    {
		this.checkStart = checkStart;
		this.checkEnd = checkEnd;
		this.cameraStart = cameraStart;
		this.cameraEnd = cameraEnd;
		this.zoomStart = zoomStart;
		this.zoomEnd = zoomEnd;
	}
}
