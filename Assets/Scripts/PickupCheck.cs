using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCheck : MonoBehaviour
{

	public AudioClip allpelletsgotten;

    // Update is called once per frame
    void Update()
    {
		GameObject[] Pickups = GameObject.FindGameObjectsWithTag ("Pickup");
		int PickupCount = Pickups.Length;

		if (PickupCount == 0) {

			GameObject[] gos = GameObject.FindGameObjectsWithTag ("PelletGate");
			foreach (GameObject go in gos)
				Destroy (go);
			GameSoundManagement.instance.PlayOneShot (allpelletsgotten);
		}
		
    }
}
