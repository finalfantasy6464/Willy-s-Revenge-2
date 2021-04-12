using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Aggroactivate : MonoBehaviour
{
	public bool active = false;
	public Transform player;
	public float range;
	private AIDestinationSetter pathing;


	void Start (){

		pathing = GetComponent<AIDestinationSetter> ();
	}

    // Update is called once per frame
    void Update()
    {
		if (player) {
			if (Vector3.Distance (transform.position, player.transform.position) <= range) {
				active = true;
			}

			if (active == false) {
				pathing.enabled = false;
			} else if (active == true) {
				pathing.enabled = true;
			}
		}
}
}
