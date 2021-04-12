using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
	public bool active = false;
	public Transform player;
	public float range;
	private EnemyProjectile projectile;


	void Start (){

		projectile = GetComponent<EnemyProjectile> ();
	}

	// Update is called once per frame
	void Update()
	{
		if (player) {
			if (Vector3.Distance (transform.position, player.transform.position) <= range) {
				active = true;
			} else {
				active = false;
			}

			if (active == false) {
				projectile.enabled = false;
			} else if (active == true) {
				projectile.enabled = true;
			}
		}
	}
}
