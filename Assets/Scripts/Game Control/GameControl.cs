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

    public float ArenahighScore;

    public int currentCharacterSprite = 0;

    public bool lastSceneWasLevel = false;
    public bool returntoselect = false;
    public bool bosscheckpoint = false;
    public bool faded = false;
    public bool InitialGameStarted = false;
    public bool justloaded = false;

    public int totallevels;
    public int targetLevels;
    public int levelID;
    public int savedPinID;

    [Header("Camera")]
    public Vector3 savedCameraPosition;
    public float savedOrtographicSize;
    public OverworldProgressView progressView; 
    public Color savedCameraBackgroundColor;

    [Header("World")]
    public float completionPercent;
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

    [Header("Save / Load")]
    public ScriptablePlayerSettings settings;
    public ScriptableGameState gameState;
    public ScriptableArenaState arenaState;

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
        if(pause != null)
        {
            pause.enabled = control.m_Scene.name.Contains("Level");
            if (pause.enabled)
            {
                pause.Initialise();
            }
        }
    }

    public void CompletionPercentageCheck()
    {
        completionPercent = 0f;

        for(int i = 0; i < completedlevels.Count; i++)
        {
            if(completedlevels[i] == true)
            {
                completionPercent += 0.33f;
            }

            if(goldenpellets[i] == true)
            {
                completionPercent += 0.33f;
            }

            if(timerchallenge[i] == true)
            {
                completionPercent += 0.33f;
            }
        }

        if(completionPercent >= 99f)
        {
            completionPercent = 100f;
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

    public void OverworldLevelStateUpdate()
    {
        MapManager mapManager = FindObjectOfType<MapManager>();
        mapManager?.InitializeLevelState();
        OverworldCamera overworldCamera = FindObjectOfType<OverworldCamera>();
        overworldCamera?.SetFromSaved(savedCameraPosition, savedOrtographicSize, savedCameraBackgroundColor);
        mapManager?.UpdateWorldGates();
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

        if(savedPinPosition == character.currentPin.transform.position && savedPinPosition != null)
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

    public IEnumerator DelayedChangeCharacterPin()
    {
        float timeoutTime = 3f;
        float timeoutCounter = 0f;
        bool timedOut = true;
        OverworldCharacter character = null;
        MapManager map = null;
        OverworldCamera overworldCamera = null;

        while(timeoutCounter < timeoutTime)
        {
            timeoutCounter += Time.deltaTime;
            character = FindObjectOfType<OverworldCharacter>();
            map = FindObjectOfType<MapManager>();
            overworldCamera = FindObjectOfType<OverworldCamera>();

            if(character == null || map == null || overworldCamera == null)
                yield return null;
            else
            {
                timedOut = false;
                break;
            }
        }

        if(timedOut)
        {
            Debug.LogError("Overworld never loaded ):");
            yield break;
        }

        overworldCamera.SetFromSaved(savedCameraPosition, savedOrtographicSize, savedCameraBackgroundColor);

        if(AutosavePosition == character.currentPin.transform.position)
            yield break;

        for (int i = 0; i < map.levelPins.Count; i++)
        {
            if(map.levelPins[i].transform.position == AutosavePosition)
            {
                character.currentPin.onCharacterExit.Invoke();
                character.SetCurrentPin(map.levelPins[i]);
                overworldCamera.SetFromPin(character.currentPin);
                character.currentPin.onCharacterEnter.Invoke();
                break;
            }
        }

        map.InitializeLevelState();
        map.UpdateWorldGates();
    }

    void SetFromGameState()
    {
        complete = gameState.complete;
        golden = gameState.golden;
        timer = gameState.timer;
        ArenahighScore = gameState.arenaScore;

        savedPinPosition = gameState.savedPinPosition;
        AutosavePosition = gameState.AutosavePosition;
        completedlevels = new List<bool>(gameState.completedlevels);
        goldenpellets = new List<bool>(gameState.goldenpellets);
        timerchallenge = new List<bool>(gameState.timerchallenge);
        lockedgates = new List<bool>(gameState.lockedgates);
        destroyedgates = new List<bool>(gameState.destroyedgates);
        lockedgatescache = new List<bool>(gameState.lockedgatescache);
        destroyedgatescache = new List<bool>(gameState.destroyedgatescache);
        progressView = gameState.progressView;
        currentCharacterSprite = gameState.characterSkinIndex;

        savedCameraPosition = gameState.savedCameraPosition;
        savedOrtographicSize = gameState.savedOrtographicSize;
        savedCameraBackgroundColor = gameState.backgroundColor;

        InitialGameStarted = gameState.initialGameStarted;
    }

    public void Save(int saveSlot)
    {
        if(progressView == OverworldProgressView.None)
            progressView = OverworldProgressView.WorldLeft;

        savedCameraBackgroundColor = FindObjectOfType<OverworldCamera>().gameCamera.backgroundColor;
        gameState.SetFromGameControl(control);
        gameState.WriteToManual(saveSlot);
        settings.SaveToDisk();
    }

    public void AutoSave()
    {
        if (progressView == OverworldProgressView.None)
            progressView = OverworldProgressView.WorldLeft;

        gameState.SetFromGameControl(control);
        gameState.WriteToAuto();
        settings.SaveToDisk();
    }

    public void Load(int saveSlot)
	{
        if(gameState.SetFromManual(saveSlot))
        {
            SetFromGameState();
            settings.TryLoadFromDisk();
        }

       if(m_Scene.name == "Overworld")
        {
            ResolutionOptions Res = GameObject.Find("OptionsPanel").GetComponent<ResolutionOptions>();
            OverworldMusicSelector music = GameObject.Find("OverworldMusic").GetComponent<OverworldMusicSelector>();
            music.overworldmusicCheck();
            Res.SetResolution(settings.resolutionWidth, settings.resolutionHeight);
            StartCoroutine(ChangeCharacterPin());
            OverworldLevelStateUpdate();
            OverworldCharacter Player = GameObject.FindGameObjectWithTag("Player").GetComponent<OverworldCharacter>();
            Player.GetComponent<SpriteRenderer>().sprite = Player.skinSprites[currentCharacterSprite];
        }
    }

    public void AutoLoad()
    {
        if(gameState.SetFromAuto())
        {
            SetFromGameState();
            settings.TryLoadFromDisk();
        }

        if(m_Scene.name == "MainMenu")
        {
            StartCoroutine(DelayedChangeCharacterPin());
        }
    }

    public void Delete(int saveSlot)
    {
        gameState.DeleteFile(ScriptableGameState.MANUAL_SAVE_PATHS[saveSlot]);
    }

    public void CheckForDeletion(int saveSlot)
    {
        gameState.DeleteManualSave(saveSlot);
    }
}

public enum OverworldProgressView
{
    None,
    WorldLeft,
    WorldRight,
    WorldFull,
    Clouds,
    Moon,
    UFO
}