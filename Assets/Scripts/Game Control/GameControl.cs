using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameControl : MonoBehaviour
{
	public static GameControl control;

	public int complete;
	public int golden;
	public int timer;

    public int camerachoice;

	public bool returntoselect = false;

	public List<bool> completedlevels = new List<bool>();
	public List<bool> goldenpellets = new List<bool>();
	public List<bool> timerchallenge = new List<bool>();

	public int totallevels;

	public int targetLevels;

	public int levelID;

	public string currentlevel; 

	Scene m_Scene;
	public string sceneName;
    
    void Awake()
    {

		levelID = 0;

		totallevels = SceneManager.sceneCountInBuildSettings;
		targetLevels = totallevels;
		m_Scene = SceneManager.GetActiveScene ();
		currentlevel = m_Scene.name;
			for (int k = 0; k < totallevels; k++) {
				if (!currentlevel.Contains("Level")) {
				targetLevels--;
		} 

		totallevels = targetLevels;
		for (int i = 1; i < totallevels + 1; i++) {
			completedlevels.Add(false);
			goldenpellets.Add (false);
			timerchallenge.Add (false);
		}

		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {
			Destroy (gameObject);
		}

            
    }
    }

    private void Start()
    {
        if(m_Scene.name == "OverWorld")
        {
        StartCoroutine(Setcamerasroutine());
        }
    }
    
    void OnGUI()
	{
		GUI.Label (new Rect(10,10,100,30), "Complete: " + complete);
		GUI.Label (new Rect(10,40,150,30), "Golden: " + golden);
	}

    [HideInInspector]public IEnumerator Setcamerasroutine()
    {
        yield return 0;
        SetCamera(camerachoice);
    }

    public void SetCamera(int index)
    {
        camerachoice = index;
        CameraController controller = GameObject.Find("OverworldCameraController").GetComponent<CameraController>();
        controller.cameras[camerachoice].gameObject.SetActive(true);
        for (int i = 0; i < controller.cameras.Count; i++)
        {
            if (camerachoice == i) continue;
            controller.cameras[i].gameObject.SetActive(false);
        }
    }


	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playersave.wr2");

		PlayerData data = new PlayerData ();
		data.complete = complete;
		data.golden = golden;
		data.timer = timer;
		data.levelID = levelID;
		data.completedlevels = completedlevels;
		data.goldenpellets = goldenpellets;
		data.timerchallenge = timerchallenge;
        data.camerachoice = camerachoice;

		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load()
	{
		if (File.Exists (Application.persistentDataPath + "/playersave.wr2"))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open(Application.persistentDataPath + "/playersave.wr2", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close ();

			complete = data.complete;
			golden = data.golden;
			timer = data.timer;
			levelID = data.levelID;
			completedlevels = data.completedlevels;
			goldenpellets = data.goldenpellets;
			timerchallenge = data.timerchallenge;
            camerachoice = data.camerachoice;

            StartCoroutine(Setcamerasroutine());
		}
	}
}

[Serializable]
class PlayerData
{
	public int complete;
	public int golden;
	public int timer;
	public int levelID;
    public int camerachoice;

	public List<bool> completedlevels;
	public List<bool> goldenpellets;
	public List<bool> timerchallenge;
}
