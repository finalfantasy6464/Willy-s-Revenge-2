using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletCollision : MonoBehaviour
{
	public GameObject Player;
	void OnCollisionEnter2D(Collision2D col){

		var hit = col.gameObject;

		if(hit.tag == "Player"){

			Destroy (gameObject);
		
}
}
}

