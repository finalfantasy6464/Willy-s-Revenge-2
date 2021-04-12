using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
	public bool usetimer = false;
	public float spinamount;
	private float spintimer;
	public int currentdir = 1;

    // Update is called once per frame
    void Update ()
	{

		spintimer += Time.deltaTime;

		if (usetimer == false) {
			transform.Rotate (0, 0, spinamount);
		}

		if (usetimer == true) {
			switch (currentdir) {
			case 2:
				if (spintimer < 1.0f) {
					transform.Rotate (0, 0, -spinamount);
				}
				if (spintimer >= 1.0f) {
					currentdir = 1;
					spintimer = 0.0f;
				}
				break;
			case 1: 
				if (spintimer < 1.0f) {
					transform.Rotate (0, 0, spinamount);
				}
				if (spintimer >= 1.0f) {
					currentdir = 2;
					spintimer = 0.0f;
				}
				break;
			}

		}
	}
}

