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
    public int complete;
    public int golden;
    public int timer;

    public int currentCharacterSprite = 0;

    public bool lastSceneWasLevel = false;
    public bool returntoselect = false;
    public bool bosscheckpoint = false;
    public bool faded = false;
    public bool InitialGameStarted = false;

    public int totallevels;
    public int targetLevels;
    public int levelID;
    public int savedPinID;
    public string currentlevel;
    public string sceneName;
    public Vector3 savedPinPosition;
    public Vector3 AutosavePosition;
    public LevelPin savedPin;

    public List<bool> completedlevels = new List<bool>();
    public List<bool> goldenpellets = new List<bool>();
    public List<bool> timerchallenge = new List<bool>();
    public List<bool> lockedgates = new List<bool>();
    public List<bool> destroyedgates = new List<bool>();
    public List<bool> lockedgatescache = new List<bool>();
    public List<bool> destroyedgatescache = new List<bool>();

    public static UnityEvent onSingletonCheck;
    public static GameControl control;
    Scene m_Scene;

    void Awake()
    {
        levelID = 0;
        totallevels = SceneManager.sceneCountInBuildSettings;
        m_Scene = SceneManager.GetActiveScene();
        currentlevel = m_Scene.name;

        if(onSingletonCheck == null)
            onSingletonCheck = new UnityEvent();

        if (control != null && control != this)
        {
            onSingletonCheck.Invoke();
            if (m_Scene.name == "MainMenu")
            {
                Destroy(control.gameObject);
            }
            else
            {
                Destroy(gameObject);
                PauseControlCheck();
                return;
            }
        }   
        DontDestroyOnLoad(gameObject);
        control = this;
        onSingletonCheck.Invoke();
        LevelListGeneration();
    }

    void PauseControlCheck()
    {
        PauseControl pause = control.GetComponent<PauseControl>();
        control.m_Scene = SceneManager.GetActiveScene();
        pause.enabled = control.m_Scene.name.Contains("Level");
        if (pause.enabled)
        {
            pause.Initialise();
        }
    }

    public void InitializeOverworldMap(List<GatePin> gates)
    {
        lockedgates.Clear();
        destroyedgates.Clear();

        if (lockedgatescache.Count == 0)
        {
            for (int i = 0; i < gates.Count; i++)
            {
                lockedgates.Add(true);
                destroyedgates.Add(false);
            }
        }
        else
        {
            for (int i = 0; i < lockedgatescache.Count; i++)
            {
                lockedgates.Add(false);
                destroyedgates.Add(false);
                lockedgates[i] = lockedgatescache[i];
                destroyedgates[i] = destroyedgatescache[i];
            }
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        GameInput.Update();
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

    void OverworldLevelStateUpdate()
    {
        MapManager mapManager = FindObjectOfType<MapManager>();
        mapManager?.InitializeLevelState();
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

    public static void CompletedLevelCheck(int levelID, bool gotGold, bool timerExpired)
    {
        control.bosscheckpoint = false;
        control.savedPinID = levelID;

        if (!control.completedlevels[levelID])
        {
            control.completedlevels[levelID] = true;
            control.complete++;
        }
        if (!control.goldenpellets[levelID] && gotGold)
        {
            control.goldenpellets[levelID] = true;
            control.golden++;
        }

        if (!control.timerchallenge[levelID] && !timerExpired)
        {
            control.timerchallenge[levelID] = true;
            control.timer++;
        }
    }

    public IEnumerator SetWorldGates()
    {
        yield return 3;
        MapManager map = FindObjectOfType<MapManager>();
        if(map != null)
        {
            map.SetWorldGateData(lockedgatescache, destroyedgatescache);
            for (int i = 0; i < map.worldGates.Count; i++)
            {
                map.worldGates[i].SetOrbState(lockedgatescache[i], destroyedgatescache[i]);
            }
        }  
    }

    public IEnumerator ChangeCharacterPin()
    {
        OverworldCharacter character = FindObjectOfType<OverworldCharacter>();
        MapManager map = FindObjectOfType<MapManager>();

        if(character == null || map == null)
            yield break;

        if(savedPinPosition == character.currentPin.transform.position)
            yield break;

        for (int i = 0; i < map.levelPins.Count; i++)
        {
            if(map.levelPins[i].transform.position == savedPinPosition)
            {
                character.currentPin.onCharacterExit.Invoke();
                character.SetCurrentPin(map.levelPins[i]);
                character.currentPin.onCharacterEnter.Invoke();
            }
        }
    }

    public void AutoSave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/autosave.wr2");
        PlayerData data = new PlayerData(complete, golden, timer, levelID, currentCharacterSprite, new SerializedVector3(AutosavePosition), new SerializedVector3Pin(savedPinPosition), completedlevels, goldenpellets, timerchallenge, lockedgates, destroyedgates, InitialGameStarted);
        bf.Serialize(file, data);
        file.Close();
    }

    public void Save()
	{
        OverworldCharacter character = FindObjectOfType<OverworldCharacter>();
        savedPinPosition = character.transform.position;
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playersave.wr2");
        PlayerData data = new PlayerData(complete, golden, timer, levelID, currentCharacterSprite, new SerializedVector3(character.transform.position),
                new SerializedVector3Pin(savedPinPosition), completedlevels,
                goldenpellets, timerchallenge, lockedgates, destroyedgates, InitialGameStarted);

        bf.Serialize (file, data);
		file.Close();
	}

    public void Load()
	{
        LoadFromFile("/playersave.wr2");

        if(m_Scene.name == "Overworld")
        {
            Debug.Log("Starting coroutine");
            StartCoroutine(ChangeCharacterPin());
            OverworldLevelStateUpdate();
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
            savedPinPosition.x = data.savedPinposition.x;
            savedPinPosition.y = data.savedPinposition.y;
            savedPinPosition.z = data.savedPinposition.z;
            lockedgatescache = data.lockedgates;
            destroyedgatescache = data.destroyedgates;
            InitialGameStarted = data.InitialGameStarted;

            lockedgates = lockedgatescache;
            destroyedgates = destroyedgatescache;
 
            StartCoroutine(SetWorldGates());
        }
    }

    public void CheckForDeletion()
    {
        DeleteFiles("/autosave.wr2");
        DeleteFiles("/playersave.wr2");
    }

    public void DeleteFiles(string localPath)
    {
        if(File.Exists(Application.persistentDataPath + localPath))
        {
            File.Delete(Application.persistentDataPath + localPath);
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
    public int currentCharacterSprite;

    public SerializedVector3 playerposition;
    public SerializedVector3Pin savedPinposition;

    public bool InitialGameStarted;

	public List<bool> completedlevels;
	public List<bool> goldenpellets;
	public List<bool> timerchallenge;
    public List<bool> lockedgates;
    public List<bool> destroyedgates;

    public PlayerData(int complete, int golden, int timer, int levelID, int currentCharacterSprite,
            SerializedVector3 serializedPosition, SerializedVector3Pin serializedPinPosition, bool InitialGameStarted)
    {
        this.complete = complete;
        this.golden = golden;
        this.timer = timer;
        this.levelID = levelID;
        this.currentCharacterSprite = currentCharacterSprite;
        this.playerposition = serializedPosition;
        this.savedPinposition = serializedPinPosition;
        this.InitialGameStarted = InitialGameStarted;
    }

    public PlayerData(int complete, int golden, int timer, int levelID, int currentCharacterSprite,
            SerializedVector3 serializedPosition, SerializedVector3Pin serializedPinPosition, List<bool> completedLevels,
            List<bool> goldenPellets, List<bool> timerChallenge, List<bool> lockedgates, List<bool> destroyedgates, bool InitialGameStarted)
    {
        this.complete = complete;
        this.golden = golden;
        this.timer = timer;
        this.levelID = levelID;
        this.currentCharacterSprite = currentCharacterSprite;
        this.playerposition = serializedPosition;
        this.savedPinposition = serializedPinPosition;
        this.completedlevels = completedLevels;
        this.goldenpellets = goldenPellets;
        this.timerchallenge = timerChallenge;
        this.lockedgates = lockedgates;
        this.destroyedgates = destroyedgates;
        this.InitialGameStarted = InitialGameStarted;
    }
}