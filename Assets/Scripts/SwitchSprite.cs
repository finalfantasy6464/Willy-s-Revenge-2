using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSprite : MonoBehaviour
{

	private PlayerController2021remake playercontroller;
	GameObject player;

	void Awake(){
		player = GameObject.FindGameObjectWithTag ("Player");
		playercontroller = player.GetComponent<PlayerController2021remake>();
		}
	void OnTriggerEnter2D(Collider2D coll) {

		var sprite = coll.gameObject;

		if (sprite.tag == "Player") {
			playercontroller.dirlock = true;
		}
	}
	void OnTriggerExit2D(Collider2D coll) {

			var sprite = coll.gameObject;

			if (sprite.tag == "Player") {
				playercontroller.dirlock = false;
			}
	}
}