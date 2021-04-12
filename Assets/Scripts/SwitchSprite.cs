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
	void OnCollisionEnter2D(Collision2D coll) {

		var sprite = coll.gameObject;

		if (sprite.tag == "Player") {
			playercontroller.dirlock = true;
		}
	}
	void OnCollisionExit2D(Collision2D coll) {

			var sprite = coll.gameObject;

			if (sprite.tag == "Player") {
				playercontroller.dirlock = false;
			}
	}
}