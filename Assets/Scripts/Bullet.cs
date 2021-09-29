using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour, IPausable
{
	PlayerController2021remake playercontroller;
	GameObject Player;
	Rigidbody2D rb;
	Collider2D myCollider;
	Vector2 storedForce;

    public bool isPaused { get; set; }

    void Start()
	{
		Player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D>();
		if (Player != null)
		{
			playercontroller = Player.GetComponent<PlayerController2021remake>();
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if(isPaused) return;

		var hit = coll.gameObject;

		if (hit.tag == "Arena")
		{
			Destroy(gameObject);
		}

		if (hit.tag == "Enemy5")
		{
			Destroy(gameObject);
		}

		if (hit.tag == "Bullet")
		{
			Destroy(gameObject);
		}

		if (hit.tag == "Bulletblocker")
		{
			Destroy(gameObject);
		}

		if (playercontroller != null)
		{
			if (playercontroller.shieldactive == false)
			{
				if (hit.tag == "Player")
				{
					Destroy(hit);
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
			}

			if (playercontroller.shieldactive == true)
			{
				if (hit.tag == "Player")
				{
					Destroy(gameObject);
				}
			}
			else
			{
			}
		}
	}

    public void OnPause()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

	public void SetForce(Vector2 f)
	{
		storedForce = f;
	}

    public void OnUnpause()
    {
        rb.constraints = RigidbodyConstraints2D.None;
		rb.AddForce(storedForce);
    }

    public void PausedUpdate() {}

    public void UnPausedUpdate() {}

	public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}