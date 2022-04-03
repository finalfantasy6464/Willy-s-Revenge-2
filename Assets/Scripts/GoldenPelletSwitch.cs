using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenPelletSwitch : MonoBehaviour
{
    BigOrange orangeScript;
    public GameObject orange;

	public Transform spawnPos;

	public GameObject Golden;

	private int pickuptotal;

	private bool goldspawned = false;

    public SpriteRenderer s_renderer;
    Color newcolor;

    private void Start()
    {
        orangeScript = orange.GetComponent<BigOrange>();
        s_renderer = gameObject.GetComponent<SpriteRenderer>();
        newcolor = s_renderer.color;
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
            newcolor.a = 0.25f;
            s_renderer.color = newcolor;
		}
    }
}
