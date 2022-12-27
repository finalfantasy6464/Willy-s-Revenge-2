using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;

public class MapManager : MonoBehaviour
{
    public OverworldGUI overworldGUI;
    public CanvasGroup FadeImageCanvasGroup;
    public overworldProgressUpdate worldProgress;

    //Pin Information//
	public OverworldLevelPin startPin;
	public OverworldLevelPin targetPin;
    public OverworldLevelPin previousPin;
    public List<OverworldLevelPin> levelPins;
    public List<OverworldGate> overworldGates;

    //Player Information//
    public OverworldPlayer player;
    public CinematicCameraTransitionHelper cameraHelper;
    public ScriptablePlayerSettings settings;
    public ResolutionOptions resolution;

    //UI Information//
    public Button backButton;
    public Button playButton;
    public Slider musicSlider;
    public Slider soundSlider;

    //Sound Information//
    public AudioClip backsound;
    public AudioClip playsound;
    public GameSoundManagement soundManagement;
    public MusicManagement musicManagement;
    public OverworldMusicSelector overworldMusic;

    private void Start ()
	{
        soundManagement = FindObjectOfType<GameSoundManagement>();
        musicManagement = FindObjectOfType<MusicManagement>();

        soundManagement.slider = soundSlider;
        musicManagement.slider = musicSlider;

        SetSlidersFromSettings();
        SetResolutionFromSettings();

        StartCoroutine(FadeInRoutine());
        
        musicManagement.onLevelStart.Invoke();

        InitializeLevelState();
    }
    IEnumerator CheckGameControl()
    {
        yield return null;

        if (GameControl.control.lastSceneWasLevel)
        {
            GameControl.control.savedOverworldPlayerPosition = levelPins[GameControl.control.savedPinID - 1].transform.position + new Vector3(0, -2f, 0);
            GameControl.control.savedCameraPosition = levelPins[GameControl.control.savedPinID - 1].transform.position + new Vector3(0, -2f, -100f);
            GameControl.control.lastSceneWasLevel = false;
        }

        if (GameControl.control.savedPin != null)
        {
            startPin = GameControl.control.savedPin;
        }

        UpdateOverworldMusic(GameControl.control.overworldMusicProgress);
        UpdateWorldView(GameControl.control.currentWorldView);
        UpdatePlayerPosition();
    }

    IEnumerator FadeInRoutine()
    {
        float fadecounter = -0.25f;
        float fadetimer = 1.5f;
        player.input.DeactivateInput();
        StartCoroutine(CheckGameControl());
        while (fadecounter <= fadetimer)
        {
            fadecounter += Time.deltaTime;
            FadeImageCanvasGroup.alpha = Mathf.Lerp(1, 0, fadecounter / fadetimer);
            yield return null;
        }
        player.input.ActivateInput();
    }

    public void UpdateOverworldMusic(int index)
    {
        overworldMusic.currentProgress = index;
        overworldMusic.overworldmusicCheck();
    }

    public void UpdateWorldView(int index)
    {
        worldProgress.UpdateWorldView(index);
        
        cameraHelper.followCamera.overworldCamera.backgroundColor = GameControl.control.backgroundColor;
        cameraHelper.cinematicCamera.backgroundColor = GameControl.control.backgroundColor;
        player.UpdateCharacterSprite();
    }

    private void SetResolutionFromSettings()
    {
        resolution.SetResolution(settings.resolutionWidth, settings.resolutionHeight);
    }

    private void SetSlidersFromSettings()
    {
        musicSlider.value = settings.bgmVolume;
        soundSlider.value = settings.sfxVolume;
    }

    public void InitializeLevelState()
    {
        for (int i = 1; i < GameControl.control.completedlevels.Count - 1; i++)
        {
            bool isComplete = GameControl.control.completedlevels[i];
            bool isGolden = GameControl.control.goldenpellets[i];
            bool isTimer = GameControl.control.timerchallenge[i];
        }
    }

    public void UpdatePlayerPosition()
    {
        player.gameObject.transform.position = GameControl.control.savedOverworldPlayerPosition;
        cameraHelper.followCamera.gameObject.transform.position = GameControl.control.savedCameraPosition;
        cameraHelper.followCamera.freeRoamTargetZoomCache = GameControl.control.savedOrtographicSize;
        GameControl.control.AutoSave();
    }

    public void UpdateWorldGates()
    {
        foreach (OverworldGate gate in overworldGates)
        {
            gate.SetPlateState(false);
            gate.SetGateState();
        }
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene($"Level{levelNumber + 1}");
    }

    public void LoadLevelFromSceneIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void UpdateLevelPinProgress()
    {
        foreach (OverworldLevelPin pin in levelPins)
        {
            pin.view.ViewProgressCheck();
        }
    }
}
