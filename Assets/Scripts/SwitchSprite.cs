using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSprite : MonoBehaviour
{

	private PlayerController playercontroller;
	GameObject player;
	private Vector2 currentdir;

	void Awake(){
		player = GameObject.Find ("Player");
		playercontroller = player.GetComponent<PlayerController>();
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