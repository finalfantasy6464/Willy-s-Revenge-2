using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Aimedbullet : MonoBehaviour, IPausable
{
	Rigidbody2D rb;

	PlayerController2021remake playercontroller;

	PlayerCollision playercoll;

	public Vector2 moveDirection;
    public Vector2 storedForce;



	public LocalAudioPlayer localaudio;

	PositionalSoundData soundData;

	public AudioClip bounceoff;

    public bool isPaused { get; set; }

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody2D> ();
		playercontroller = GameObject.FindObjectOfType<PlayerController2021remake> ();
		playercoll = GameObject.FindObjectOfType<PlayerCollision>();
	    soundData = localaudio.soundData;
	}

    // Update is called once per frame
	void OnTriggerEnter2D(Collider2D coll)
	{
		var hit = coll.gameObject;
		string tag = coll.gameObject.tag;

		if (tag == "Arena" || tag == "Enemy5" || tag == "Bullet" || tag == "Switch2")
			Destroy (gameObject);

		if (playercontroller == null)
			return;

		if (!playercontroller.shieldactive)
		{
			if (hit.tag == "Player" && playercoll.canbehit)
			{
				Destroy(hit);
				SceneManager.LoadScene(SceneManager.GetActiveScene ().name);
			}
		}

		if (playercontroller.shieldactive)
		{
			if (hit.tag == "ActiveShield")
            {
				StartCoroutine(BulletDeflect());
			}
		}
	}

	IEnumerator BulletDeflect()
	{
		soundData.clip = bounceoff;
		localaudio.SoundPlay();
		this.GetComponent<SpriteRenderer>().enabled = false;
		this.GetComponent<Collider2D>().enabled = false;
		this.GetComponentInChildren<ParticleSystem>().Stop();
		yield return new WaitForSeconds(2);
		Destroy(gameObject);
	}

	public void SetForce(Vector2 f)
    {
		storedForce = f;
    }

	public void OnPause()
	{
		storedForce = rb.velocity;
		if (rb != null && rb.constraints != RigidbodyConstraints2D.FreezeAll)
		{
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
		}
	}

	public void OnUnpause()
    {
		rb.constraints = RigidbodyConstraints2D.None;
		rb.AddForce(storedForce, ForceMode2D.Impulse);
	}

    public void OnDestroy()
    {
		PauseControl.TryRemovePausable(gameObject);
    }

	public void UnPausedUpdate()
	{ }

}
