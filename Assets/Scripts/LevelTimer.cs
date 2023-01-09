using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour, IPausable
{
    public int levelIndex;
	public float timermin;
	public float timersec;

	public float goaltime;

    public float leveltime;

	public TextMeshProUGUI text;

	public bool expired = false;

    public LevelList levelList;

    LevelCanvas canvas;

    public bool isCounting = true;

    public bool isPaused { get; set; }

    #region fuckoffanddie
    public void OnPause() {}

    public void OnUnpause() {}
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        goaltime = levelList.parTimes[levelIndex - 1];
		timersec = 0.0f;
        canvas = GameObject.FindObjectOfType<LevelCanvas>();
        text = canvas.timerText;
    }

    void Update()
    {
        if (!isPaused)
            UnPausedUpdate();
    }

    public void UnPausedUpdate()
    {
        if (isCounting)
        {
            leveltime += Time.deltaTime;
        }
        text.text = Mathf.Floor(leveltime / 60).ToString("00") + ":" + ((int)leveltime % 60).ToString("00");

        if (leveltime >= goaltime)
        {
            expired = true;
            text.color = new Color(0.9f, 0.2f, 0.2f);
        }
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}

