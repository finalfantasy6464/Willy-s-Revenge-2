using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IPausable
{

	PlayerController2021remake playercontroller;
	GameObject Player;
	public float shieldtimer = 5.0f;

    public AudioClip shieldActive;

	public AudioSource source;

    public bool isPaused { get; set; }

    void Start(){
		Player = GameObject.FindGameObjectWithTag ("Player");
		playercontroller = Player.GetComponent<PlayerController2021remake>();
        source.PlayOneShot(shieldActive);
	}

	void Update(){
        if (!isPaused)
        {
            UnPausedUpdate();
        }
	}

    public void OnPause()
    {
        GetComponent<ParticleSystem>().Pause();
    }

    public void OnUnpause()
    {
        GetComponent<ParticleSystem>().Play();
    }

    public void PausedUpdate()
    { }

    public void UnPausedUpdate()
    {
        shieldtimer -= Time.deltaTime;

        if (shieldtimer <= 0.0f)
        {
            playercontroller.shieldactive = false;
            Destroy(this.gameObject);
            source.Stop();
        }
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}