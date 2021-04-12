using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
	private CheckpointManager cm;

	void Start(){
		cm = GameObject.FindGameObjectWithTag ("CM").GetComponent<CheckpointManager> ();
		transform.position = cm.lastCheckPointPos;
	}
}
