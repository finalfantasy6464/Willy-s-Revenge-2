using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour {

	private int platformcounter = 0;

	private float safetytimer = 0.0f;

	private bool Safe = true;
	private bool isPlatform = false;

	public AudioClip keyget;
	public AudioClip enemyhit;
	public AudioClip gatehit;
	public AudioClip lavaburn;
	public AudioClip falling;

	public delegate void MyDelegate();
	public event MyDelegate onDeath;

	void Update(){

		safetytimer += Time.deltaTime;

		if (platformcounter == 0 & isPlatform == true) {
			Destroy (gameObject);
			DontDestroyme.instance.PlayOneShot (falling);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

	void OnCollisionEnter2D(Collision2D col){

		var hit = col.gameObject;


		if(hit.tag == "Gate"){
			Destroy(gameObject);
			DontDestroyme.instance.PlayOneShot (gatehit);
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}

		if(hit.tag == "Gate2"){
			Destroy(gameObject);
			DontDestroyme.instance.PlayOneShot (gatehit);
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}

		if(hit.tag == "PelletGate"){
			Destroy(gameObject);
			DontDestroyme.instance.PlayOneShot (gatehit);
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}
		if (hit.tag == "Key") {

			GameObject[] gos = GameObject.FindGameObjectsWithTag ("Gate");
			foreach (GameObject go in gos)
				Destroy (go);
			Destroy (hit);
			DontDestroyme.instance.PlayOneShot (keyget);
		}

		if (hit.tag == "Key2") {

			GameObject[] gos = GameObject.FindGameObjectsWithTag ("Gate2");
			foreach (GameObject go in gos)
				Destroy (go);
			Destroy (hit);
			DontDestroyme.instance.PlayOneShot (keyget);
		}

		if (hit.tag == "Tail") {
			Destroy (gameObject);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

		if (hit.tag == "Enemy") {
			Destroy (gameObject);
			DontDestroyme.instance.PlayOneShot (gatehit);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

		if (hit.tag == "Enemy2") {
			Destroy (gameObject);
			DontDestroyme.instance.PlayOneShot (gatehit);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

		if (hit.tag == "Enemy3") {
			Destroy (gameObject);
			DontDestroyme.instance.PlayOneShot (gatehit);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

		if (hit.tag == "Enemy5") {
			Destroy (gameObject);
			DontDestroyme.instance.PlayOneShot (gatehit);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
		if (hit.tag == "Oneway") {
			Destroy (gameObject);
			DontDestroyme.instance.PlayOneShot (gatehit);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}
	void OnTriggerEnter2D(Collider2D trig){

		var hit = trig.gameObject;
		if(hit.tag == "Enemy3"){
			Destroy(gameObject);
			DontDestroyme.instance.PlayOneShot (gatehit);
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}

		if (hit.tag == "Colour") {
			Destroy (gameObject);
			DontDestroyme.instance.PlayOneShot (gatehit);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

		if (hit.tag == "Safety"){
			Safe = true;
			safetytimer = 0.0f;
}

		if (hit.tag == "Platform") {
			platformcounter++;
			isPlatform = true;
		}

		if (hit.tag == "Disable"){
			isPlatform = false;
	}
	}

	void OnTriggerExit2D (Collider2D exit){
		var hit = exit.gameObject;
		var all = exit.gameObject;
		if (hit.tag == "Safety"){
			Safe = false;
			safetytimer = 0.0f;
		}

		if (hit.tag == "Platform"){
			platformcounter--;
		}
	}

	void OnTriggerStay2D(Collider2D stay){
		var hit = stay.gameObject;
		if (hit.tag == "Danger" & Safe == false & safetytimer >= 0.1f){
			safetytimer = 0.0f;
			Destroy (gameObject);
			DontDestroyme.instance.PlayOneShot (lavaburn);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
}

	
}

	void Death()
	{
		onDeath.Invoke();
	}
}