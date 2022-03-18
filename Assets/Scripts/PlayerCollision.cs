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
	public float skullFloatTime;
	public GameObject corruptionObject;
	public Color corruptionColor;
	public event MyDelegate onDeath;
	
	public int platformcounter = 0;
	float safetytimer = 0.0f;
	bool Safe = true;
	bool isPlatform = false;
	bool justcollided = false;
	[HideInInspector]
	public bool dyingByCorruption = false;
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

    void LateUpdate()
	{
		if(dyingByCorruption)
			return;

		safetytimer += Time.deltaTime;

		if (platformcounter == 0 && isPlatform == true && playerCollider.enabled && LavaWorld == false) {
			spriteRenderer.enabled = false;
			Die(onFalling);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

        if (platformcounter == 0 && isPlatform == true && playerCollider.enabled && LavaWorld == true)
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
		dyingByCorruption = true;
		float counter = 0f;
		float time = corruptionDeathTime;
		List<GameObject> segments = playerController.taillist;
		List<SpriteRenderer> segmentRenderers = playerController.tailListRenderers;
		Vector3 deathPosition = transform.position;
		Vector3 direction = playerController.direction;
		Vector3 startScale = playerCollider.transform.localScale;
		playerController.corruptionDirectionCache = direction;
		playerCollider.enabled = false;
		corruptionObject.SetActive(true);
		canbehit = false;

        for (int i = 0; i < playerController.taillist.Count; i++)
		{
			playerController.tailListColliders[i].isTrigger = false;
			segments[i].GetComponent<Animator>().enabled = false;
		}

		while (counter < time)
		{
			counter += Time.deltaTime;
			transform.position = Vector3.Lerp(deathPosition, deathPosition + (direction * 0.3f), counter / time);
			transform.localScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, counter / time);
			spriteRenderer.color = Color.Lerp(Color.white, corruptionColor, counter / time);
			playerController.FinalMovementVector = playerController.FinalMovementVector.normalized;
			yield return null;
		}

		spriteRenderer.enabled = false;
		yield return new WaitForSeconds(afterDeathSleepTime);
		transform.localScale = startScale;
		playerAnimator.SetTrigger("OnSkullFloat");
		yield return null;
		while(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
			yield return null;
		
		playerAnimator.enabled = false;

		float floatCounter = 0f;

		while(floatCounter < skullFloatTime)
		{
			floatCounter += Time.deltaTime;
			yield return null;
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

	public IEnumerator CorruptionDeathRoutine(SpriteRenderer segmentRenderer)
    {
        float counter = 0f;
        Vector3 startScale = segmentRenderer.transform.localScale;
		Color segmentColorEnd = corruptionColor;
		segmentColorEnd.a = 0f;
        while(counter < corruptionDeathTime)
        {
            counter += Time.deltaTime;
            segmentRenderer.transform.localScale = Vector3.Lerp(startScale, startScale * 0.66f, counter / corruptionDeathTime);
            segmentRenderer.color = Color.Lerp(Color.white, segmentColorEnd, (counter * 2f) / corruptionDeathTime);
            yield return null;
        }
    }

	void Death()
	{
		onDeath.Invoke();
	}
}