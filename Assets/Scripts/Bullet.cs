using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{

	PlayerController2021remake playercontroller;
	GameObject Player;

	void Start(){

		Player = GameObject.FindGameObjectWithTag ("Player");
		if (Player != null) {
			playercontroller = Player.GetComponent<PlayerController2021remake> ();
		}
	}

	void OnTriggerEnter2D (Collider2D coll){

	var hit = coll.gameObject;

		if (hit.tag == "Arena"){
			Destroy (gameObject);
	}

		if (hit.tag == "Enemy5"){
			Destroy (gameObject);
		}

		if (hit.tag == "Bullet") {
			Destroy (gameObject);
		}

        if (hit.tag == "Bulletblocker")
        {
            Destroy(gameObject);
        }
			
		if (playercontroller != null) {
			if (playercontroller.shieldactive == false) {
				if (hit.tag == "Player") {
					Destroy (hit);
					SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
				}
			}

			if (playercontroller.shieldactive == true) {
				if (hit.tag == "Player") {
					Destroy (gameObject);
				}
			} else {
			}
	}
}
}