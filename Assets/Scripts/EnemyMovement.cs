using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovement : MonoBehaviour
{

	private float movespeed = 0.0f;
    private float moveinterval = 0.1f;
	public float multiplier = 1.0f;

	private Vector2 enemydir = Vector2.right;
    private bool levelstart = false;
	private bool justhit = false;
	public int direction = 1;



    void Start()
    {
        StartCoroutine(LevelStarting());
    }

    IEnumerator LevelStarting()
    {
        if (levelstart == false)
        {
            yield return new WaitForSeconds(0.2f);
            levelstart = true;
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
			
		while (direction == 1) {
				
			if (justhit == true) {
				break;
			} else {
				
				if ((coll.gameObject.tag != "Enemy") & (coll.gameObject.tag != "Teleport")) {

					direction = 2;
					justhit = true;
				}

				if (coll.gameObject.tag == "Teleport") {

					justhit = true;
				}

				if (coll.gameObject.tag == "Enemy") {
					Destroy (this.gameObject);
					justhit = true;
				}
			}
		}

		while (direction == 2) {
				
			if (justhit == true) {
				break;
			} else {

				if ((coll.gameObject.tag != "Enemy") & (coll.gameObject.tag != "Teleport")) {

					direction = 1;
					justhit = true;
				}

				if (coll.gameObject.tag == "Teleport") {

					justhit = true;
				}
				if (coll.gameObject.tag == "Enemy") {
					Destroy (this.gameObject);
					justhit = true;
				}
			}
		}
	}

     void Update(){

        if (levelstart == true) {
        this.movespeed += Time.deltaTime * multiplier;
        }
		

		if (this.movespeed >= moveinterval) {
			Move ();
            movespeed = movespeed - moveinterval;
		}

		switch (direction) {

		case 2:
			enemydir = Vector2.left / 8;
			break;
				
		case 1:
			
			enemydir = Vector2.right / 8;
			break;
		}
	}

	void LateUpdate(){
		justhit = false;
	}

	void Move() {

		Vector3 v = transform.position;

		transform.Translate (enemydir);
	}
}