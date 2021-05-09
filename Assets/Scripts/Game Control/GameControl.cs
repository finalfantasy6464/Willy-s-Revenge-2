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

    public bool bosscheckpoint = false;

    public List<bool> completedlevels = new List<bool>();
    public List<bool> goldenpellets = new List<bool>();
    public List<bool> timerchallenge = new List<bool>();

    public int totallevels;

    public int targetLevels = 0;

    public int levelID;

    public string currentlevel;

    Scene m_Scene;
    public string sceneName;

    Pin savedPin;

    public Vector3 AutosavePosition;

    void Awake()
    {

        levelID = 0;

        totallevels = SceneManager.sceneCountInBuildSettings;

        m_Scene = SceneManager.GetActiveScene();
        currentlevel = m_Scene.name;


        if (control == null) {
            DontDestroyOnLoad(gameObject);
            control = this;

            for (int k = 0; k < totallevels; k++)
            {
                if (currentlevel.Contains("Level"))
                {
                    targetLevels++;
                }
            }

            for (int i = 1; i < targetLevels + 1; i++)
            {
                completedlevels.Add(false);
                goldenpellets.Add(false);
                timerchallenge.Add(false);
            }

        } else if (control != this) {
            Destroy(gameObject);
        }


    }

    private void Start()
    {
        if (m_Scene.name == "OverWorld")
        {
            StartCoroutine(Setcamerasroutine());
        }

        else if (m_Scene.name == "MainMenu")
        {
            for (int i = 0; i < completedlevels.Count; i++)
            {
                completedlevels[i] = false;
                goldenpellets[i] = false;
                timerchallenge[i] = false;
            }
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), "Complete: " + complete);
        GUI.Label(new Rect(10, 40, 150, 30), "Golden: " + golden);
    }

    [HideInInspector] public IEnumerator Setcamerasroutine()
    {
        yield return 0;
        SetCamera(camerachoice);
    }

    [HideInInspector] public IEnumerator ChangeCharacterPin()
    {
        GameObject CharacterObject = GameObject.Find("Character");


        while (CharacterObject == null)
        {
            yield return 0;
        }

        Character character = CharacterObject.GetComponent<Character>();

        character.transform.position = control.savedPin.transform.position;
        character.SetCurrentPin(control.savedPin);
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


    public void AutoSave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/autosave.wr2");

        PlayerData data = new PlayerData();
        data.complete = complete;
        data.golden = golden;
        data.timer = timer;
        data.levelID = levelID;
        data.completedlevels = completedlevels;
        data.goldenpellets = goldenpellets;
        data.timerchallenge = timerchallenge;
        data.camerachoice = camerachoice;
        data.playerposition = new SerializedVector3(AutosavePosition);

        bf.Serialize(file, data);
        file.Close();
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
        data.playerposition = new SerializedVector3(GameObject.Find("Character").transform.position);

        Character character = GameObject.Find("Character").GetComponent<Character>();

        Pin pin;

        foreach (Collider2D result in Physics2D.OverlapPointAll(character.transform.position))
        {
            pin = result.GetComponent<Pin>();
            if (pin != null)
            {
                character.SetCurrentPin(pin);
                savedPin = pin;
            }
        }

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

            control.StartCoroutine(Setcamerasroutine());
            control.StartCoroutine(ChangeCharacterPin());
		}
	}

    public void AutoLoad()
    {
        if (File.Exists(Application.persistentDataPath + "/autosave.wr2"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/autosave.wr2", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            complete = data.complete;
            golden = data.golden;
            timer = data.timer;
            levelID = data.levelID;
            completedlevels = data.completedlevels;
            goldenpellets = data.goldenpellets;
            timerchallenge = data.timerchallenge;
            camerachoice = data.camerachoice;

            control.StartCoroutine(Setcamerasroutine());
            control.StartCoroutine(ChangeCharacterPin());
        }
    }
}

[Serializable]
public class SerializedVector3
{
    public float x, y, z;
    public SerializedVector3(Vector3 Pos)
    {
        x = Pos.x;
        y = Pos.y;
        z = Pos.z;
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
    public SerializedVector3 playerposition;

	public List<bool> completedlevels;
	public List<bool> goldenpellets;
	public List<bool> timerchallenge;
}
