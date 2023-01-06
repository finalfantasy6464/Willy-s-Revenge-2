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
    public bool autoloadSuccessful = false;
    public bool justloaded = false;

    public int totallevels;
    public int targetLevels;
    public int levelID;
    public int savedPinID;

    [Header("Camera")]
    public Vector3 savedCameraPosition;
    public float savedOrtographicSize;

    [Header("World")]
    public float completionPercent;
    public string currentlevel;
    public string sceneName;
    public int overworldMusicProgress;
    public int currentWorldView;
    public Color backgroundColor;

    public Vector3 savedOverworldPlayerPosition;
    public Color playerColor;
    public Vector3 playerLocalScale;
    public float playerMoveSpeed;
    public Vector3 AutosavePosition;
    public OverworldLevelPin savedPin;

    public List<bool> completedlevels = new List<bool>();
    public List<bool> goldenpellets = new List<bool>();
    public List<bool> timerchallenge = new List<bool>();
    public List<bool> destroyedgates = new List<bool>();

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

    public void LoadIntoOverworld()
    {
        SceneManager.LoadScene("Overworld");
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

    void SetFromGameState()
    {
        complete = gameState.complete;
        golden = gameState.golden;
        timer = gameState.timer;
        ArenahighScore = gameState.arenaScore;

        completedlevels = new List<bool>(gameState.completedlevels);
        goldenpellets = new List<bool>(gameState.goldenpellets);
        timerchallenge = new List<bool>(gameState.timerchallenge);
        destroyedgates = new List<bool>(gameState.destroyedgates);

        overworldMusicProgress = gameState.overworldMusicProgress;
        currentWorldView = gameState.currentWorldView;
        backgroundColor = gameState.backgroundColor;

        currentCharacterSprite = gameState.characterSkinIndex;
        savedOrtographicSize = gameState.savedOrtographicSize;
        completionPercent = gameState.completionPercent;

        savedCameraPosition = gameState.savedCameraPosition;
        savedOverworldPlayerPosition = gameState.savedOverworldPlayerPosition;
        playerColor = gameState.playerColor;
        playerLocalScale = gameState.playerLocalScale;
        playerMoveSpeed = gameState.playerMoveSpeed;
        AutosavePosition = gameState.AutosavePosition;

        autoloadSuccessful = gameState.autoloadSuccessful;
    }

    public void Save(int saveSlot)
    {
        gameState.SetFromGameControl(control);
        gameState.WriteToManual(saveSlot);
        settings.SaveToDisk();
    }

    public void AutoSave()
    {
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
            Res.SetResolution(settings.resolutionWidth, settings.resolutionHeight);
            OverworldPlayer Player = GameObject.FindGameObjectWithTag("Player").GetComponent<OverworldPlayer>();
            Player.canMove = true;
            MapManager map = GameObject.FindObjectOfType<MapManager>();
            map.UpdateOverworldState(currentWorldView, overworldMusicProgress);
        }
    }

    public void AutoLoadCheck()
    {
        if(gameState.SetFromAuto())
        {
            SetFromGameState();
            settings.TryLoadFromDisk();
            autoloadSuccessful = true;
        }
        else
            autoloadSuccessful = false;
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