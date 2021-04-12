using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

	PlayerController playercontroller;
	GameObject Player;
	private float shieldtimer = 5.0f;


	void Start(){
		Player = GameObject.FindGameObjectWithTag ("Player");
		playercontroller = Player.GetComponent<PlayerController> ();
	}

	void Update(){
		shieldtimer -= Time.deltaTime;

		if (shieldtimer < 0.0f) {
			playercontroller.shieldactive = false;
			Destroy (this.gameObject);
		}
	}
}