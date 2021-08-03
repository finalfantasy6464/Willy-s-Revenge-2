using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DoorOpener : MonoBehaviour
{

	GameObject[] Shooter;
	GameObject[] Enemy5;
	GameObject Door;
	AIChaser autodoor;
	private MovingShooterArrow[] arrow;

	private Aggroactivate[] aggro;

	void Start() {

		Door = GameObject.FindGameObjectWithTag("Door");
		autodoor = Door.GetComponent<AIChaser>();
		Shooter = GameObject.FindGameObjectsWithTag("Shooter");
		Enemy5 = GameObject.FindGameObjectsWithTag("Enemy5");
		arrow = new MovingShooterArrow[Shooter.Length];
		aggro = new Aggroactivate[Enemy5.Length];
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

			for (int k = 0; k < Enemy5.Length; k++)
			{
				aggro[k] = Enemy5[k].GetComponent<Aggroactivate>();
				aggro[k].range = 10;
			}


			autodoor.Active = true;
}
	}
}
