using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tailcheck : MonoBehaviour
{

	private bool isplatform = false;

	private int platformcounter = 0;

	void Update(){
		if (platformcounter == 0 & isplatform == true) {
			Destroy (gameObject);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

	void OnTriggerEnter2D(Collider2D coll){

		var hit = coll.gameObject;

	if (hit.tag == "PZ2") {
		platformcounter++;
		isplatform = true;
	}
			}

	void OnTriggerExit2D(Collider2D exit){

		var hit = exit.gameObject;

		if (hit.tag == "PZ2"){
			platformcounter--;
		}
	}


}
