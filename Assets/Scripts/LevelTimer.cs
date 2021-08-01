using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{

	public float timermin;
	public float timersec;

	public float goaltime;

	public Text text;

	public bool expired = false;

    // Start is called before the first frame update
    void Start()
    {
		timersec = 0.0f;
    }

    // Update is called once per frame
    void Update()
	{
		text.text = Mathf.Floor(Time.timeSinceLevelLoad / 60).ToString ("00") + ":" + ((int)Time.timeSinceLevelLoad % 60).ToString("00");

		if (Time.timeSinceLevelLoad >= goaltime) {
			expired = true;
			text.color = new Color (0.4f, 0.4f, 0.4f);

		}
    }
}

