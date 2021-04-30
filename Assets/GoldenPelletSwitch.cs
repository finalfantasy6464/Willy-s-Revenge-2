using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenPelletSwitch : MonoBehaviour
{
    BigOrange orangeScript;
    GameObject orange;

	public Transform spawnPos;

	public GameObject Golden;

	private int pickuptotal;

	private bool goldspawned = false;

    private void Start()
    {
        orange = GameObject.FindGameObjectWithTag("Boss");
        orangeScript = orange.GetComponent<BigOrange>();
    }

    private void Update()
    {
        if(orangeScript.HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        var hit = coll.gameObject;

		GameObject[] totalpickups = GameObject.FindGameObjectsWithTag("Pickup");
		pickuptotal = totalpickups.Length;

		if (pickuptotal == 0 & goldspawned == false && hit.tag == ("Player")) {
			GameObject Goldenpel = Instantiate (Golden, spawnPos);
			goldspawned = true;
		}
    }
}
