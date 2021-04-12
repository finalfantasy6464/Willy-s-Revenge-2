using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
	public Quaternion openRotation;
	public Quaternion closedRotation;
	bool Open = false;
	public float openspeed = 1.0f;
	public float changetimer = 0.0f;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

		changetimer += Time.deltaTime;

		if (changetimer >= 1.0f & Open == true) {
			transform.rotation = Quaternion.Slerp (transform.rotation, closedRotation, openspeed);
			changetimer = 0.0f;
			Open = false;
		}

		if (changetimer >= 1.0f & Open == false) {
			transform.rotation = Quaternion.Slerp (transform.rotation, openRotation, openspeed);
			changetimer = 0.0f;
			Open = true;
		}
    }
}

