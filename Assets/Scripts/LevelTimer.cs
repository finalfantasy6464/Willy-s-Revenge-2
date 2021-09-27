using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour, IPausable
{

	public float timermin;
	public float timersec;

	public float goaltime;

    public float leveltime;

	public Text text;

	public bool expired = false;

    public bool isPaused { get; set; }

    #region fuckoffanddie
    public void OnPause() {}

    public void OnUnpause() {}

    public void PausedUpdate() {}
    #endregion

    public void UnPausedUpdate()
    {
        leveltime += Time.deltaTime;
        text.text = Mathf.Floor(leveltime / 60).ToString("00") + ":" + ((int)leveltime % 60).ToString("00");

        if (leveltime >= goaltime)
        {
            expired = true;
            text.color = new Color(0.4f, 0.4f, 0.4f);
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
	
}

