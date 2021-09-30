using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Aimedbullet : MonoBehaviour, IPausable
{

	public float movespeed;

	Rigidbody2D rb;

	PlayerController2021remake playercontroller;

	PlayerCollision playercoll;

	public Vector2 moveDirection;
    public Vector2 storedForce;

    public bool isPaused { get; set; }

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody2D> ();
		playercontroller = GameObject.FindObjectOfType<PlayerController2021remake> ();
		playercoll = GameObject.FindObjectOfType<PlayerCollision>();
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
	public void SetForce(Vector2 f)
    {
		storedForce = f;
    }

	public void OnPause()
	{
		if (rb != null)
		{
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
		}
	}

	public void OnUnpause()
    {
		Debug.Log(gameObject);
		rb.constraints = RigidbodyConstraints2D.None;
		rb.AddForce(storedForce);
	}

    public void OnDestroy()
    {
		PauseControl.TryRemovePausable(gameObject);
    }

	public void PausedUpdate()
	{ }

	public void UnPausedUpdate()
	{ }

}
