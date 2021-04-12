using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawblade : MonoBehaviour{

	private Vector3 startPosition;
	public float Severity = 1.0f;
	public float speed = 1.0f;
	public int Direction = 1; 
	public float rotspeed = 4.0f;

	void Start()
	{
		startPosition = transform.position;
	}
    // Update is called once per frame
    void Update()
    {
			transform.Rotate (0,0,rotspeed);

		switch (Direction) {

		case 8:
			transform.position = startPosition + new Vector3 (-Mathf.Sin (Time.timeSinceLevelLoad * speed) * Severity, -Mathf.Sin (Time.timeSinceLevelLoad * speed) * Severity, 0.0f);
			break;
		case 7:
			transform.position = startPosition + new Vector3 (Mathf.Sin (Time.timeSinceLevelLoad * speed) * Severity, -Mathf.Sin (Time.timeSinceLevelLoad * speed) * Severity, 0.0f);
			break;
		case 6:
			transform.position = startPosition + new Vector3 (-Mathf.Sin (Time.timeSinceLevelLoad * speed) * Severity, Mathf.Sin (Time.timeSinceLevelLoad * speed) * Severity, 0.0f);
			break;
		case 5:
			transform.position = startPosition + new Vector3 (Mathf.Sin (Time.timeSinceLevelLoad * speed) * Severity, Mathf.Sin (Time.timeSinceLevelLoad * speed) * Severity, 0.0f);
			break;
		case 4:
			transform.position = startPosition - new Vector3 (0.0f, Mathf.Sin (Time.timeSinceLevelLoad * speed) * Severity, 0.0f);
			break;
		case 3:
			transform.position = startPosition - new Vector3 (Mathf.Sin (Time.timeSinceLevelLoad * speed) * Severity, 0.0f, 0.0f);
			break;
		case 2:
			transform.position = startPosition + new Vector3 (Mathf.Sin (Time.timeSinceLevelLoad * speed) * Severity, 0.0f, 0.0f);
			break;

		case 1:
			transform.position = startPosition + new Vector3 (0.0f, Mathf.Sin (Time.timeSinceLevelLoad * speed) * Severity, 0.0f);
			break;
		}

    }
}
