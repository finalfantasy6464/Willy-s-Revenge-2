using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayerCollision : MonoBehaviour {

	private int platformcounter = 0;

	private float safetytimer = 0.0f;

	private bool Safe = true;
	private bool isPlatform = false;

	private bool justcollided = false;

    public bool LavaWorld = false;

	public delegate void MyDelegate();
	public event MyDelegate onDeath;

	public Collider2D playerCollider;
	public SpriteRenderer spriterenderer;

	string[] hostileStrings;

    [HideInInspector] public UnityEvent onKeyCollect;
    [HideInInspector] public UnityEvent onWallCollide;
    [HideInInspector] public UnityEvent onLavaBurn;
    [HideInInspector] public UnityEvent onFalling;
    [HideInInspector] public UnityEvent onElectricHit;
	
    void Awake()
    {
        onKeyCollect = new UnityEvent();
        onWallCollide = new UnityEvent();
        onLavaBurn = new UnityEvent();
        onFalling = new UnityEvent();
        onElectricHit = new UnityEvent();

		hostileStrings = new string[]
		{
			"Arena",
			"Boss",
			"Colour",
			"Enemy",
			"Enemy2",
			"Enemy3",
			"Enemy5",
			"Gate",
			"Gate2",
			"Oneway",
			"PelletGate"
		};
    }

    void Update(){

		safetytimer += Time.deltaTime;

		if (platformcounter == 0 & isPlatform == true && LavaWorld == false) {
			spriterenderer.enabled = false;
			Die(onFalling);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

        if (platformcounter == 0 & isPlatform == true && LavaWorld == true)
        {
			spriterenderer.enabled = false;
			Die(onLavaBurn);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

	void OnCollisionEnter2D(Collision2D col){

		var hit = col.gameObject;
		if (hostileStrings.Any(s => hit.tag.Equals(s)) && justcollided == false)
        {
			Die(onWallCollide);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		
		if (hit.tag == "Key") {

			GameObject[] gos = GameObject.FindGameObjectsWithTag ("Gate");
			foreach (GameObject go in gos)
				Destroy (go);
			Destroy (hit);
			onKeyCollect.Invoke();
		}

		if (hit.tag == "Key2") {

			GameObject[] gos = GameObject.FindGameObjectsWithTag ("Gate2");
			foreach (GameObject go in gos)
				Destroy (go);
			Destroy (hit);
            onKeyCollect.Invoke();
        }

		if (hit.tag == "Tail") {
			Destroy (gameObject);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}
	void OnTriggerEnter2D(Collider2D trig){

		var hit = trig.gameObject;
		if (hostileStrings.Any(s => hit.tag.Equals(s)) && justcollided == false)
		{
			Die(onWallCollide);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

	void OnTriggerStay2D(Collider2D stay)
	{
		var hit = stay.gameObject;
		if (hit.tag == "Danger" & Safe == false & safetytimer >= 0.1f)
		{
			safetytimer = 0.0f;
			spriterenderer.enabled = false;
			onLavaBurn.Invoke();
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	public void Die(UnityEvent chosenevent)
    {
		chosenevent.Invoke();
	    spriterenderer.enabled = false;
		foreach(follower segment in GetComponent<PlayerController>().taillist)
        {
			segment.gameObject.SetActive(false);
        }
		justcollided = true;
	}

	void Death()
	{
		    onDeath.Invoke();
	}
}