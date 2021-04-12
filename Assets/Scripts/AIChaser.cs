using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaser : MonoBehaviour
	{

	public float speed = 0.5f;
	public Transform Player;
	public bool Active = false;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (Active) {
			if (Player) {
				Vector3 displacement = Player.position - transform.position;
				displacement = displacement.normalized;
				if (Vector2.Distance (Player.position, transform.position) > 0.1f) {
					transform.position += (displacement * speed * Time.deltaTime);
					transform.up = -Player.position + transform.position;
				}
			}
		}
		else{

			Active = false;
			//do whatever the enemy has to do with the player
		}

	}
	}
