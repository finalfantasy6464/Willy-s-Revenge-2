using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimerArena : MonoBehaviour, IPausable
{
	public float timermin;
	public float timersec;
    public float startingLevelTime;
    public float currentLevelTime;
    public bool timerLock = true;
	public Text text;
	public bool gameOver = false;

    public Canvas ScoreCanvas;
    public ScoreScreenCanvas score;
    public ArenaController arenaControl;
    public PlayerController2021Arena arena;
    public PlayerCollisionArena arenacoll;

    public bool isPaused { get; set; }

    #region fuckoffanddie
    public void OnPause() {}

    public void OnUnpause() {}

    #endregion

    // Start is called before the first frame update
    void Start()
    {
		timersec = 0.0f;
        currentLevelTime = startingLevelTime;
        text.text = Mathf.Floor(currentLevelTime / 60).ToString("00") + ":" + ((int)currentLevelTime % 60).ToString("00");
    }

    public void Reset()
    {
        timerLock = false;
        currentLevelTime = startingLevelTime;
    }

    public void UnlockTimer()
    {
        timerLock = false;
    }

    public void UnPausedUpdate()
    {
        if (!timerLock)
        {
            currentLevelTime = Mathf.Max(0, currentLevelTime - Time.deltaTime);
            text.text = Mathf.Floor(currentLevelTime / 60).ToString("00") + ":" + ((int)currentLevelTime % 60).ToString("00");

            if (currentLevelTime <= 0)
            {
                currentLevelTime = 0;
                score.TitleText.SetText("Time Up!");
                arenacoll.Die(arenacoll.onWallCollide);
            }
        }
    }

    void Update()
    {
        if (!isPaused)
            UnPausedUpdate();
    }
	public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}

