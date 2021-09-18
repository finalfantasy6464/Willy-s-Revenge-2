using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
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
    public bool faded = false;

    public List<bool> completedlevels = new List<bool>();
    public List<bool> goldenpellets = new List<bool>();
    public List<bool> timerchallenge = new List<bool>();
    public List<bool> lockedgates = new List<bool>();
    public List<bool> destroyedgates = new List<bool>();
    public List<bool> lockedgatescache = new List<bool>();
    public List<bool> destroyedgatescache = new List<bool>();

    public int totallevels;
    public int targetLevels;
    public int levelID;
    public string currentlevel;
    Scene m_Scene;
    public string sceneName;
    public Pin savedPin;

    public Vector3 savedPinPosition;
    public Vector3 AutosavePosition;

    public static UnityEvent onSingletonCheck;

    public Character character;

    void Awake()
    {
        levelID = 0;
        totallevels = SceneManager.sceneCountInBuildSettings;
        m_Scene = SceneManager.GetActiveScene();
        currentlevel = m_Scene.name;

        if(onSingletonCheck == null)
        {
            onSingletonCheck = new UnityEvent();
        }
        bool InitialSingleton = control == null;

        if (!InitialSingleton && control != this)
        {
            onSingletonCheck.Invoke();
            if (m_Scene.name == "MainMenu")
            {
                Destroy(control.gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }   
        DontDestroyOnLoad(gameObject);
        control = this;
        onSingletonCheck.Invoke();
        LevelListGeneration();
    }
 

    private void Start()
    {
        Application.targetFrameRate = 60;

        if (m_Scene.name == "Overworld")
        {
            StartCoroutine(Setcamerasroutine());
        }   
    } 

    public void LevelListGeneration()
    {
        completedlevels.Clear();
        goldenpellets.Clear();
        timerchallenge.Clear();

        string pathToScene;
        string sceneName;

        for (int k = 0; k < SceneManager.sceneCountInBuildSettings; k++)
      {
           pathToScene = SceneUtility.GetScenePathByBuildIndex(k);
           sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);

            if (sceneName.Contains("Level"))
            {
                targetLevels++;
            }        
      }
     for (int i = 0; i < targetLevels + 1; i++)
      {
            completedlevels.Add(false);
            goldenpellets.Add(false);
            timerchallenge.Add(false);
      }
    }

    public void CallCameraRoutine()
    {
        StartCoroutine(Setcamerasroutine());
    }

    public IEnumerator LoadRoutine(int routinechoice)
    {
        yield return 3;

        if(routinechoice == 1)
        {
            control.Load();
        }

        if(routinechoice == 2)
        {
            control.AutoLoad();
        }
    }

    public IEnumerator Setcamerasroutine()
    {
        yield return 3;
        SetCamera(camerachoice);
    }

    [HideInInspector]
    public IEnumerator SetWorldGates()
    {
        yield return 3;
        MapManager map = GameObject.Find("MapManager").GetComponent<MapManager>();
        map.SetWorldGateData(lockedgatescache, destroyedgatescache);
        for (int i = 0; i < map.worldgates.Count; i++)
        {
            map.worldgates[i].SetOrbState(lockedgatescache[i], destroyedgatescache[i]);
        }
    }

    [HideInInspector] public IEnumerator ChangeCharacterPin()
    {
        GameObject CharacterObject = GameObject.Find("Character");

        if(CharacterObject == null)
        {
            yield break;
        }

        character = CharacterObject.GetComponent<Character>();
        character.SetPinPosition();
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
        PlayerData data = new PlayerData(complete, golden, timer, levelID,
                camerachoice, new SerializedVector3(AutosavePosition), new SerializedVector3Pin(savedPinPosition), completedlevels, goldenpellets, timerchallenge, lockedgates, destroyedgates);
        bf.Serialize(file, data);
        file.Close();
    }

    public void Save()
	{
        Character character = GameObject.Find("Character").GetComponent<Character>();
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playersave.wr2");
        SetPinFromPosition(character);
        PlayerData data = new PlayerData(complete, golden, timer, levelID,
                camerachoice, new SerializedVector3(character.transform.position), new SerializedVector3Pin(savedPinPosition), completedlevels, goldenpellets, timerchallenge, lockedgates, destroyedgates);
        bf.Serialize (file, data);
		file.Close();
	}

    public void SetPinFromPosition(Character character)
    {
        Pin pin;
        foreach (Collider2D result in
                Physics2D.OverlapPointAll(character.transform.position))
        {
            pin = result.GetComponent<Pin>();
            if (pin == null) continue;
            character.SetCurrentPin(pin);
            savedPin = pin;
            savedPinPosition = pin.transform.position;
        }
    }

    public void Load()
	{
        LoadFromFile("/playersave.wr2");

        if(m_Scene.name == "Overworld")
        {
            Debug.Log("Starting coroutine");
            StartCoroutine(ChangeCharacterPin());
        }
    }

    public void AutoLoad()
    {
        LoadFromFile("/autosave.wr2");
    }

    public void LoadFromFile(string localPath)
    {
        if (File.Exists(Application.persistentDataPath + localPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + localPath, FileMode.Open);
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
            savedPinPosition.x = data.savedPinposition.x;
            savedPinPosition.y = data.savedPinposition.y;
            savedPinPosition.z = data.savedPinposition.z;
            lockedgatescache = data.lockedgates;
            destroyedgatescache = data.destroyedgates;
 
            StartCoroutine(SetWorldGates());
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
public class SerializedVector3Pin
{
    public float x, y, z;
    public SerializedVector3Pin(Vector3 Pos)
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
    public SerializedVector3Pin savedPinposition;

	public List<bool> completedlevels;
	public List<bool> goldenpellets;
	public List<bool> timerchallenge;
    public List<bool> lockedgates;
    public List<bool> destroyedgates;

    public PlayerData(int complete, int golden, int timer, int levelID, int cameraChoice,
            SerializedVector3 serializedPosition, SerializedVector3Pin serializedPinPosition)
    {
        this.complete = complete;
        this.golden = golden;
        this.timer = timer;
        this.levelID = levelID;
        this.camerachoice = cameraChoice;
        this.playerposition = serializedPosition;
        this.savedPinposition = serializedPinPosition;
    }

    public PlayerData(int complete, int golden, int timer, int levelID, int cameraChoice,
            SerializedVector3 serializedPosition, SerializedVector3Pin serializedPinPosition, List<bool> completedLevels,
            List<bool> goldenPellets, List<bool> timerChallenge, List<bool> lockedgates, List<bool> destroyedgates)
    {
        this.complete = complete;
        this.golden = golden;
        this.timer = timer;
        this.levelID = levelID;
        this.camerachoice = cameraChoice;
        this.playerposition = serializedPosition;
        this.savedPinposition = serializedPinPosition;
        this.completedlevels = completedLevels;
        this.goldenpellets = goldenPellets;
        this.timerchallenge = timerChallenge;
        this.lockedgates = lockedgates;
        this.destroyedgates = destroyedgates;
    }
}