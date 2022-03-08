using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

public class PlayerCollision : MonoBehaviour
{
    public bool LavaWorld = false;
	public bool canbehit = true;
	public delegate void MyDelegate();
	public PlayerController2021remake playerController;
	public SpriteRenderer spriteRenderer;
	public Collider2D playerCollider;
	public Animator playerAnimator;
	[Header("Corruption")]
	public float corruptionDeathTime;
	public float afterDeathSleepTime;
	public GameObject corruptionObject;
	public Color corruptionColor;
	public event MyDelegate onDeath;
	
	public int platformcounter = 0;
	float safetytimer = 0.0f;
	bool Safe = true;
	bool isPlatform = false;
	bool justcollided = false;
	string[] hostileStrings;
	IEnumerator corruptionRoutine;

    [HideInInspector] public UnityEvent onKeyCollect;
    [HideInInspector] public UnityEvent onWallCollide;
    [HideInInspector] public UnityEvent onLavaBurn;
    [HideInInspector] public UnityEvent onFalling;
    [HideInInspector] public UnityEvent onElectricHit;
    [HideInInspector] public UnityEvent onCorruptionHit;
	
    void Awake()
    {
        onKeyCollect = new UnityEvent();
        onWallCollide = new UnityEvent();
        onLavaBurn = new UnityEvent();
        onFalling = new UnityEvent();
        onElectricHit = new UnityEvent();
		onCorruptionHit = new UnityEvent();

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

    void LateUpdate(){

		safetytimer += Time.deltaTime;

		if (platformcounter == 0 & isPlatform == true && LavaWorld == false) {
			spriteRenderer.enabled = false;
			Die(onFalling);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

        if (platformcounter == 0 & isPlatform == true && LavaWorld == true)
        {
			spriteRenderer.enabled = false;
			Die(onLavaBurn);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

	void OnCollisionEnter2D(Collision2D col)
	{
		var hit = col.gameObject;

		if(canbehit == true)
        {
			if (hostileStrings.Any(s => hit.tag.Equals(s)) && justcollided == false)
			{
				Die(onWallCollide);
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}

			if (hit.tag == "Key")
			{
				GameObject[] gos = GameObject.FindGameObjectsWithTag("Gate");
				foreach (GameObject go in gos)
					Destroy(go);
				Destroy(hit);
				onKeyCollect.Invoke();
			}

			if (hit.tag == "Key2")
			{
				GameObject[] gos = GameObject.FindGameObjectsWithTag("Gate2");
				foreach (GameObject go in gos)
					Destroy(go);
				Destroy(hit);
				onKeyCollect.Invoke();
			}

			if (hit.tag == "Tail")
			{
				Destroy(gameObject);
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}

			if (hit.tag == "Exit")
			{
				canbehit = false;
				playerAnimator.SetBool("Exited", true);
			}
		}
		
	}

    void OnTriggerEnter2D(Collider2D trig)
	{
		var hit = trig.gameObject;
		if (canbehit == true)
        {
			if (hostileStrings.Any(s => hit.tag.Equals(s)) && justcollided == false)
			{
				Die(onWallCollide);
				GameSoundManagement.StopAllCurrent();
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}

			if(hit.tag == "Corruption" && corruptionRoutine == null)
			{
				corruptionRoutine = CorruptionDeathRoutine();
				StartCoroutine(corruptionRoutine);
			}

			if (hit.tag == "Safety")
			{
				Safe = true;
				safetytimer = 0.0f;
			}

			if (hit.tag == "Platform")
			{
				platformcounter++;
				isPlatform = true;
			}

			if (hit.tag == "Disable")
			{
				isPlatform = false;
			}
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
			spriteRenderer.enabled = false;
			onLavaBurn.Invoke();
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	public void Die(UnityEvent chosenevent)
    {
		chosenevent.Invoke();
	    spriteRenderer.enabled = false;

		foreach (GameObject segment in GetComponent<PlayerController2021remake>().taillist)
		{
			segment.SetActive(false);
		}

		justcollided = true;
	}

	IEnumerator CorruptionDeathRoutine()
    {
        onCorruptionHit.Invoke();
		float counter = 0f;
		float time = corruptionDeathTime;
		List<GameObject> segments = playerController.taillist;
		List<SpriteRenderer> segmentRenderers = playerController.tailListRenderers;
		Vector3 deathPosition = transform.position;
		Vector3 direction = playerController.direction;
		Vector3 scale = playerCollider.transform.localScale;
		playerController.enabled = false;
		playerCollider.enabled = false;
		corruptionObject.SetActive(true);
		canbehit = false;

		while (counter < time)
		{
			counter += Time.deltaTime;
			transform.position = Vector3.Lerp(deathPosition, deathPosition + (direction * 0.5f), counter / time);
			transform.localScale = Vector3.Lerp(scale, Vector3.one * 0.1f, counter / time);
			spriteRenderer.color = Color.Lerp(Color.white, corruptionColor, counter / time);

            for (int i = 0; i < segments.Count; i++)
			{
				Debug.Log(segmentRenderers[i]);
                segments[i].transform.localScale = Vector3.Lerp(scale, Vector3.one * 0.1f, counter / time);
                segmentRenderers[i].color = Color.Lerp(Color.white, corruptionColor, (counter * 2f) / time);
			}

			yield return null;
		}

		spriteRenderer.enabled = false;
		yield return new WaitForSeconds(afterDeathSleepTime);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

	void Death()
	{
		onDeath.Invoke();
	}
}