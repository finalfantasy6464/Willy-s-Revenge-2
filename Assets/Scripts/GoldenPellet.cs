using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenPellet : MonoBehaviour
{
	PlayerController playercontroller;
	GameObject Player;

	public Transform spawnPos;

	public GameObject Golden;

	public int pickupTotal = 5;

	private bool goldspawned = false;

    private void Start()
    {
		StartCoroutine(FindPickups());
	}

    IEnumerator FindPickups()
    {
		yield return null;
		GameObject[] totalpickups = GameObject.FindGameObjectsWithTag("Pickup");
		pickupTotal = totalpickups.Length;
		yield break;
	}

    // Update is called once per frame
    void Update()
    {
		if (pickupTotal == 0 & goldspawned == false) {
			Golden.transform.position = spawnPos.position;
			Golden.SetActive(true);
			goldspawned = true;
		}
    }
}
