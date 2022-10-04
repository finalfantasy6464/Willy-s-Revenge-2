using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenPellet : MonoBehaviour
{
	PlayerController playercontroller;
	GameObject Player;

	public Transform spawnPos;

	public GameObject Golden;

	private int pickuptotal;

	private bool goldspawned = false;

    // Update is called once per frame
    void Update()
    {
		GameObject[] totalpickups = GameObject.FindGameObjectsWithTag("Pickup");
		pickuptotal = totalpickups.Length;

		if (pickuptotal == 0 & goldspawned == false) {
			Golden.SetActive(true);
			goldspawned = true;
		}
    }
}
