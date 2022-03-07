using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MapManager : MonoBehaviour
{
	public OverworldCharacter character;
    public OverworldCamera overworldCamera;
    public OverworldGUI overworldGUI;

	public LevelPin startPin;
	public LevelPin targetPin;
    public LevelPin previousPin;

    public Button backButton;
    public Button playButton;

    public Slider musicSlider;
    public Slider soundSlider;

    public AudioClip backsound;
    public AudioClip playsound;

    public GameSoundManagement soundManagement;
    public MusicManagement musicManagement;

    public GamepadBackEnabler[] ButtonsEnabler;
    public List<LevelPin> levelPins;
    public List<GatePin> worldGates;

    public MonoBehaviour[] waypoints;
    public ObjectToggle[] toggle;

    public ScriptablePlayerSettings settings;

    public ResolutionOptions resolution;

    private void Start ()
	{
        character = FindObjectOfType<OverworldCharacter>();
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
            overworldCamera.gameCamera.backgroundColor = GameControl.control.savedCameraBackgroundColor;
            overworldCamera.viewToggler.Set(GameControl.control.progressView);

            if(GameControl.control.savedPinID > 0 && GameControl.control.savedPinID < 31)
            {
                toggle[0].ToggleBehaviour();
            }
            if (GameControl.control.savedPinID >= 31 && GameControl.control.savedPinID <= 70)
            {
                toggle[1].ToggleBehaviour();
            }
            if (GameControl.control.savedPinID >= 71 && GameControl.control.savedPinID <= 80)
            {
                toggle[2].ToggleBehaviour();
            }
            if (GameControl.control.savedPinID >= 81 && GameControl.control.savedPinID <= 90)
            {
                toggle[3].ToggleBehaviour();
            }
            if (GameControl.control.savedPinID > 90)
            {
                toggle[4].ToggleBehaviour();
            }
        }

        if (GameControl.control.savedPin != null)
        {
            startPin = GameControl.control.savedPin;
        }

        musicManagement.onLevelStart.Invoke();
        character.Initialize(startPin);
        overworldGUI.Initialize(this, character);
        
        backButton.onClick.AddListener(()=> soundManagement.PlaySingle(backsound));
        playButton.onClick.AddListener(()=> soundManagement.PlaySingle(playsound));
        InitializeWorldGates();
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

    public void UnlockAndDestroyGate(GatePin gate)
    {
        for(int i = 0; i < worldGates.Count; i++)
        {
           if(worldGates[i] == gate)
            {
                GameControl.control.lockedgates[i] = false;
                GameControl.control.destroyedgates[i] = true;
            }
        }
    }

    void InitializeWorldGates()
    {
        GatePin gate;

        for (int i = 0; i < worldGates.Count; i++)
        {
            gate = worldGates[i];

            gate.locked = GameControl.control.lockedgates[i];
            gate.destroyed = GameControl.control.destroyedgates[i];

            gate.map = this;
            gate.SetOrbState(gate.locked, gate.destroyed);
        }
    }

    public void InitializeLevelState()
    {
        for (int i = 1; i < GameControl.control.completedlevels.Count - 1; i++)
        {
            bool isComplete = GameControl.control.completedlevels[i];
            bool isGolden = GameControl.control.goldenpellets[i];
            bool isTimer = GameControl.control.timerchallenge[i];

            if (isComplete)
            {
                levelPins[i - 1].complete = true;
            }

            if (isGolden)
            {
                levelPins[i - 1].goldChallenge = true;
            }

            if (isTimer)
            {
                levelPins[i - 1].timeChallenge = true;
            }

            levelPins[i - 1].SetState();
        }
    }

    public void UpdateWorldGates()
    {
        for (int j = 0; j < worldGates.Count; j++)
        {
            if (GameControl.control.destroyedgates[j] == true)
            {
                worldGates[j].DestroyActivate();
            }
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
    
    public void OnDisable()
    {
        backButton.onClick.RemoveListener(() => soundManagement.PlaySingle(backsound));
        playButton.onClick.RemoveListener(() => soundManagement.PlaySingle(playsound));
    }
    
    public void LoadLevelFromCurrentPin()
    {
        for (int i = 0; i < levelPins.Count; i++)
        {
            if(levelPins[i] == character.currentPin)
            {
                SceneManager.LoadScene($"Level{i + 1}");
                break;
            }
        }
    }

    public void LoadLevelFromSceneIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void SetAutoSavePinPosition()
    {
        GameControl.control.savedPinPosition = character.currentPin.transform.position;
        GameControl.control.savedCameraPosition = overworldCamera.transform.position;
        GameControl.control.savedOrtographicSize = overworldCamera.gameCamera.orthographicSize;
    }
}
