using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	private CheckpointManager cm;

	void Start(){
		cm = GameObject.FindGameObjectWithTag ("CM").GetComponent<CheckpointManager> ();
	}

	void OnTriggerEnter2D (Collider2D coll){
		if (coll.CompareTag ("Player")) {
			cm.lastCheckPointPos = transform.position;
		}
	}
}
