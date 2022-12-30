using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerCollisionArena : MonoBehaviour {

	private bool justcollided = false;

	public bool canbehit = true;

	public delegate void MyDelegate();
	public event MyDelegate onDeath;

	public Canvas ScoreCanvas;
	public ScoreScreenCanvas score;
	public PlayerController2021Arena arena;
	public ArenaController arenaControl;
	public LevelTimerArena timer;

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

    void OnCollisionEnter2D(Collision2D col)
	{

		var hit = col.gameObject;

		if(canbehit == true)
        {
			if (hostileStrings.Any(s => hit.tag.Equals(s)) && justcollided == false)
			{
				Die(onWallCollide);

			}

			if (hit.tag == "Tail")
			{
				Die(onWallCollide);
			}
		}
	}
	void OnTriggerEnter2D(Collider2D trig){

		var hit = trig.gameObject;
		if (canbehit == true)
        {
			if (hostileStrings.Any(s => hit.tag.Equals(s)) && justcollided == false)
			{
				Die(onWallCollide);

			}

			if (hit.CompareTag("Tail"))
			{
				Die(onWallCollide);
			}
		}
	}

	public void Die(UnityEvent chosenevent)
    {
		chosenevent.Invoke();
	    spriterenderer.enabled = false;
		playerCollider.enabled = false;
		arena.canmove = false;

			foreach (GameObject segment in GetComponent<PlayerController2021Arena>().taillist)
			{
				segment.SetActive(false);
			}

		justcollided = true;
		arenaControl.spawnLock = true;
		timer.timerLock = true;
		ScoreCanvas.gameObject.SetActive(true);
		score.currentScore = arena.Score;
		arena.scoreLock = true;
		score.ScoreCheck();
	}

	public void ArenaReset()
    {
		justcollided = false;
		arenaControl.spawnLock = false;
		arena.scoreLock = false;
		arena.Score = 0;
		arena.pelletno = 0;
		playerCollider.enabled = true;
    }
	void Death()
	{
		onDeath.Invoke();
	}
}