using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

	PlayerController playercontroller;
	GameObject Player;
	private float shieldtimer = 5.0f;

    public AudioClip shieldActive;


	void Start(){
		Player = GameObject.FindGameObjectWithTag ("Player");
		playercontroller = Player.GetComponent<PlayerController> ();
        GameSoundManagement.instance.PlayOneShot(shieldActive);
	}

	void Update(){
		shieldtimer -= Time.deltaTime;

		if (shieldtimer < 0.0f) {
			playercontroller.shieldactive = false;
			Destroy (this.gameObject);
            GameSoundManagement.instance.efxSource.Stop();
        }
	}
}