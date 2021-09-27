using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MapManager : MonoBehaviour
{
	public OverworldCharacter character;
    public OverworldGUI overworldGUI;

	public LevelPin startPin;
	public LevelPin targetPin;
    public LevelPin previousPin;

    public Button backButton;
    public Button playButton;

    public AudioClip backsound;
    public AudioClip playsound;

    public GameSoundManagement soundManagement;
    public GamepadBackEnabler[] ButtonsEnabler;
    public List<LevelPin> levelPins;
    public List<GatePin> worldGates;

    private void Start ()
	{
        startPin = levelPins[GameControl.control.levelID];

        if(character == null)
            character = FindObjectOfType<OverworldCharacter>();
        
        if(soundManagement == null)
            soundManagement = FindObjectOfType<GameSoundManagement>();

        character.Initialize(startPin);
        overworldGUI.Initialize(this, character);
        
        soundManagement.GetComponent<MusicManagement>().onOverworld.Invoke();
        backButton.onClick.AddListener(()=> soundManagement.PlaySingle(backsound));
        playButton.onClick.AddListener(()=> soundManagement.PlaySingle(playsound));
        GameControl.control.InitializeOverworldMap(worldGates);
        InitializeWorldGates();
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
        bool isCacheEmpty = GameControl.control.lockedgatescache.Count == 0;
        GatePin gate;

        for (int i = 0; i < worldGates.Count; i++)
        {
            gate = worldGates[i];
            if (isCacheEmpty)
            {
                gate.locked = GameControl.control.lockedgates[i];
                gate.destroyed = GameControl.control.destroyedgates[i];
            }
            else
            {
                gate.locked = GameControl.control.lockedgatescache[i];
                gate.destroyed = GameControl.control.destroyedgatescache[i];
            }
            gate.map = this;
            gate.SetOrbState(gate.locked, gate.destroyed);
        }
    }

    public void SetWorldGateData(List<bool> lockedgates, List<bool> destroyedgates)
    {
        for (int i = 0; i < worldGates.Count; i++)
        {
            worldGates[i].locked = lockedgates[i];
            worldGates[i].destroyed = destroyedgates[i];
            worldGates[i].OnLevelLoaded.Invoke();
        }
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
}
