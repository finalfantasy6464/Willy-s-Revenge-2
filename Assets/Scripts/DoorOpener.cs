using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DoorOpener : MonoBehaviour
{

	GameObject[] Shooter;
	GameObject Door;
	AIChaser autodoor;
	private MovingShooterArrow[] arrow;

	void Start(){

		Door = GameObject.FindGameObjectWithTag ("Door");
		autodoor = Door.GetComponent<AIChaser> ();
		Shooter = GameObject.FindGameObjectsWithTag ("Shooter");
		arrow = new MovingShooterArrow[Shooter.Length];
	}

	void OnTriggerEnter2D (Collider2D coll){

		var hit = coll.gameObject;

		if (hit.tag == "Player"){

			GameObject[] Locks = GameObject.FindGameObjectsWithTag ("Gate2");
			foreach (GameObject Lock in Locks)
			Destroy (Lock);

			for (int i = 0; i < Shooter.Length; i++) {
				arrow [i] = Shooter [i].GetComponent<MovingShooterArrow> ();
				arrow [i].Active = true;
			}


			autodoor.Active = true;
}
	}
}
