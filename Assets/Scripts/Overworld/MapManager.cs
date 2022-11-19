using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MapManager : MonoBehaviour
{
    public OverworldGUI overworldGUI;

	public OverworldLevelPin startPin;
	public OverworldLevelPin targetPin;
    public OverworldLevelPin previousPin;

    public OverworldPlayer player;
    public OverworldFollowCamera followCamera;

    public Button backButton;
    public Button playButton;

    public Slider musicSlider;
    public Slider soundSlider;

    public AudioClip backsound;
    public AudioClip playsound;

    public GameSoundManagement soundManagement;
    public MusicManagement musicManagement;

    public List<OverworldLevelPin> levelPins;

    public MonoBehaviour[] waypoints;

    public ScriptablePlayerSettings settings;

    public ResolutionOptions resolution;

    public List<OverworldGate> overworldGates;

    private void Start ()
	{
        soundManagement = FindObjectOfType<GameSoundManagement>();
        musicManagement = FindObjectOfType<MusicManagement>();

        soundManagement.slider = soundSlider;
        musicManagement.slider = musicSlider;

        SetSlidersFromSettings();
        SetResolutionFromSettings();

        if (GameControl.control.lastSceneWasLevel)
        {
            GameControl.control.savedPin = levelPins[GameControl.control.savedPinID - 1];
            GameControl.control.lastSceneWasLevel = false;
        }

        if (GameControl.control.savedPin != null)
        {
            startPin = GameControl.control.savedPin;
        }

        musicManagement.onLevelStart.Invoke();

        InitializeLevelState();
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
        player.gameObject.transform.position = GameControl.control.savedPinPosition + new Vector3(0,-1.25f,0);
        followCamera.gameObject.transform.position = GameControl.control.savedCameraPosition + new Vector3(0, -1.25f, 0);
    }

    public void UpdateWorldGates()
    {
        foreach (OverworldGate gate in overworldGates)
        {
            gate.SetPlateState(false);
            gate.SetGateState();
        }
    }
    public void SetWorldGateData(List<bool> lockedgates, List<bool> destroyedgates)
    {
        //for (int i = 0; i < worldGates.Count; i++)
        //{
        //    worldGates[i].locked = lockedgates[i];
        //    worldGates[i].destroyed = destroyedgates[i];
        //    worldGates[i].OnLevelLoaded.Invoke();
        //}
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene($"Level{levelNumber + 1}");
    }

    public void LoadLevelFromSceneIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void SetAutoSavePinPosition()
    {

    }

    public void SetAutoSavePinPosition(OverworldPlayer character)
    {
        GameControl.control.savedPinPosition = character.currentPin.transform.position;
        //GameControl.control.savedCameraPosition = overworldCamera.transform.position;
        //GameControl.control.savedOrtographicSize = overworldCamera.gameCamera.orthographicSize;
    }
}
