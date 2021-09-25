using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Camera overworldCamera;
	public Transform sizeAnchor;
	public Transform followTarget;
	public float minDistance;
	public float maxDistance;
	public float minSize;
	public float maxSize;

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
		rawDistance = Vector2.Distance(followTarget.position, sizeAnchor.position);
		clampedDistance = Mathf.Clamp(rawDistance, minDistance, maxDistance);
        overworldCamera.orthographicSize = Mathf.Lerp(minSize, maxSize, clampedDistance / maxDistance);
    }

    void UpdatePosition()
    {
        transform.position = new Vector3(followTarget.transform.position.x,
				followTarget.transform.position.y, transform.position.z);
    }
}
