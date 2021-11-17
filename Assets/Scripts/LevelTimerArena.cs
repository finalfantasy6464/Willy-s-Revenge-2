using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimerArena : MonoBehaviour, IPausable
{
	public float timermin;
	public float timersec;
	public float goaltime;
    public float leveltime;
    public bool timerLock = false;
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

    public void PausedUpdate() {}
    #endregion

    public void UnPausedUpdate()
    {
        if (!timerLock)
        {
            leveltime = Mathf.Max(0, leveltime - Time.deltaTime);
            text.text = Mathf.Floor(leveltime / 60).ToString("00") + ":" + ((int)leveltime % 60).ToString("00");

            if (leveltime <= 0)
            {
                leveltime = 0;
                score.TitleText.text = "Time Up!";
                arenacoll.Die(arenacoll.onWallCollide);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
		timersec = 0.0f;
    }

    void Update()
    {
        if (!isPaused)
        {
            UnPausedUpdate();
        }
    }
	public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}

