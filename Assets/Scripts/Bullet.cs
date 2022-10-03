using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System;

public class Bullet : MonoBehaviour, IPausable
{
	PlayerController2021remake playercontroller;
	PlayerCollision playerColl;
	GameObject Player;
	Rigidbody2D rb;
	Collider2D myCollider;
	Vector2 storedForce;
	public AudioClip bounceoff;

	SpriteRenderer m_SpriteRenderer;

	public Animator m_Animator;

	public LocalAudioPlayer localaudio;

	PositionalSoundData soundData;

	public bool isPaused { get; set; }

    void Start()
	{
		soundData = localaudio.soundData;
		Player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D>();
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
		if (Player != null)
		{
			playercontroller = Player.GetComponent<PlayerController2021remake>();
			playerColl = Player.GetComponent<PlayerCollision>();
		}
	}

	IEnumerator BulletDeflect()
	{
		soundData.clip = bounceoff;
		localaudio.SoundPlay();
		this.GetComponent<SpriteRenderer>().enabled = false;
		this.GetComponent<Collider2D>().enabled = false;
		this.GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
		yield return new WaitForSeconds(2);
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if(isPaused) return;

		var hit = coll.gameObject;

		if (hit.tag == "Arena")
		{
			ActivateDeath();
		}

		if (hit.tag == "Bullet")
		{
			ActivateDeath();
		}

		if (hit.tag == "Bulletblocker")
		{
			ActivateDeath();
		}

		if (hit.tag == "ActiveShield")
		{
			ActivateDeath();
		}

		if (playercontroller != null)
		{
			if (playercontroller.shieldactive == false)
			{
				if (hit.tag == "Player")
				{
					playerColl.Die(playerColl.onWallCollide);
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
			}

			if (playercontroller.shieldactive == true)
			{
				if (hit.tag == "Player")
				{
					ActivateDeath();
				}
			}
			else
			{
			}
		}
	}

    private void ActivateDeath()
    {
		myCollider.enabled = false;
		m_SpriteRenderer.sprite = null;
		storedForce = Vector2.zero;
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
		m_Animator.SetTrigger("Destroy");
	}

    public void DestroySelf()
    {
		Destroy(gameObject);
    }


    public void OnPause()
    {
		if(rb != null)
        {
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
		}
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

    public void UnPausedUpdate() {}

	public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}