using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorStrike : MonoBehaviour
{
	public Vector2 center;
	public Vector2 size;

	private float trigger;
	public float firerate;

    public int firearcmin = 0;
    public int firearcmax = 360;

	public GameObject Meteor;

	void Update(){

		trigger += Time.deltaTime;

		if (trigger >= firerate) {
			SpawnMeteor ();
			trigger = 0.0f;
		}
	}

	public void SpawnMeteor(){
		Vector2 pos = center + new Vector2 (Random.Range(-size.x / 2, size.x / 2),Random.Range(-size.y / 2, size.y / 2));

		Instantiate (Meteor, pos, Quaternion.Euler(0,0,Random.Range(firearcmin,firearcmax))); 
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = new Color (0.5f, 0.0f, 0.5f, 0.25f);
		Gizmos.DrawCube (center, size);
	}
}
