using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Aimedbullet : MonoBehaviour
{

	public float movespeed;

	Rigidbody2D rb;

	PlayerController2021remake playercontroller;

	PlayerCollision playercoll;

	Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody2D> ();
		playercontroller = GameObject.FindObjectOfType<PlayerController2021remake> ();
		playercoll = GameObject.FindObjectOfType<PlayerCollision>();
        if (playercontroller != null)
        {
          moveDirection = (playercontroller.transform.position - transform.position).normalized * movespeed;
        }
		rb.velocity = new Vector2 (moveDirection.x, moveDirection.y);
    }

    // Update is called once per frame
	void OnTriggerEnter2D(Collider2D coll)
	{
	var hit = coll.gameObject;

	if (hit.tag == "Arena"){
		Destroy (gameObject);
	}

	if (hit.tag == "Enemy5"){
		Destroy (gameObject);
	}

	if (hit.tag == "Bullet") {
		Destroy (gameObject);
	}

	if (hit.tag == "Switch2") {
		Destroy (gameObject);
	}

	if (playercontroller != null) {
		if (playercontroller.shieldactive == false) {
			if (hit.tag == "Player" && playercoll.canbehit == true) {
				Destroy (hit);
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}
		}

		if (playercontroller.shieldactive == true) {
			if (hit.tag == "Player") {
				Destroy (gameObject);
			}
		} else {
		}
	}
}
}
