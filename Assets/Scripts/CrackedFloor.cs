using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrackedFloor : MonoBehaviour
{
	public Sprite Sprite2;
	bool touchedonce;

	void OnCollisionEnter2D(Collision2D coll){

		if (touchedonce == true){
		var sprite = coll.gameObject;

			if (sprite.tag == "Player") {
				Destroy (sprite);
				SceneManager.LoadScene (SceneManager.GetActiveScene().name);
			} else {
			}
	}
	}

	void OnCollisionExit2D(Collision2D coll) {

		var sprite = coll.gameObject;

		if (sprite.tag == "Player") {
			this.GetComponent<SpriteRenderer> ().sprite = Sprite2;
			touchedonce = true;
		}
	}
}
