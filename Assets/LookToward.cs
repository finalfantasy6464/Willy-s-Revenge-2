using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToward : MonoBehaviour
{

	public Transform target;

    // Update is called once per frame
    void Update()
    {
		if (target != null) {
			Vector3 difference = target.position - transform.position;
			float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
		}
	}
}
