using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<Summary>
/// The GUI does not communicate with GameControl or serialized data,
/// it instead trusts that the information in the LevelPins is correct
///</Summary>
public class OverworldGUI : MonoBehaviour
{
    public GUIWindow focusedWindow;
    [Header("Live Data")]
    public LevelPin selectedLevel;

    [Header("GUI Elements")]
    public GUIWindow pauseMenu;
    public GUIWindow levelPreview;
    public GUIWindow optionsPanel;
    public GUIWindow saveLoadPanel;
    public GUIWindow saveLoadConfirmationPanel;
    public GUIWindow tutorial_1;
    public GUIWindow tutorial_2;
    public GUIWindow tutorial_3;

    private float levelpreviewcounter;
    private float levelpreviewtime = 0.1f;

    public GUIWindow[] All => new GUIWindow[]
    {
        pauseMenu, levelPreview, optionsPanel, saveLoadPanel,
        saveLoadConfirmationPanel, tutorial_1, tutorial_2, tutorial_3
    };
    
    [HideInInspector]
    public OverworldCharacter character;
    [HideInInspector]
    public MapManager map;

    bool isAnyShowing => All.Any(w => w.isShowing);
    bool wasAnyShowing => All.Any(w => w.wasShowing);
    
    bool isTutorialShowing => tutorial_1.isShowing || tutorial_2.isShowing || tutorial_3.isShowing;
    bool wasTutorialShowing => tutorial_1.wasShowing || tutorial_2.wasShowing || tutorial_3.wasShowing;

    void Update()
    {
        character.canMove = !isAnyShowing;
        if(character.currentPin is GatePin) return;

        if(levelpreviewcounter < levelpreviewtime)
            levelpreviewcounter += Time.deltaTime;

        if(GameInput.InputMapPressedDown["select"]() && !isAnyShowing)
        {
            if (!wasAnyShowing && !character.isMoving)
            {
                LevelPreviewCheck();
            }
            else if (levelPreview.isShowing)
                 map.LoadLevelFromCurrentPin();
        }
        
        if(GameInput.InputMapPressedDown["pause"]())
        {
            if(GetShowing(out GUIWindow[] showing) > 0)
            {
                foreach (GUIWindow window in showing)
                {
                    if(window == saveLoadPanel)
                        ((SaveLoadPanel)window).ProcessCloseAll();
                    window.Close();
                }
            }
            else
                pauseMenu.Open();
            return;
        }

        if(GameInput.InputMapPressedDown["cancel"]())
        {
            if(GetShowing(out GUIWindow[] showing) > 0)
            {
                foreach (GUIWindow window in showing)
                {
                    if(window == saveLoadPanel)
                    {
                        ((SaveLoadPanel)window).ProcessCancelInput();
                        return;
                    }
                    else if (window == optionsPanel)
                    {
                        ((OptionsPanel)window).ProcessCancelInput();
                        return;
                    }
                    else if(window is GUIPrompt prompt)
                        prompt.navigationParent.Open();
                    window.Close();
                }
            }
        }
    }

    private void LevelPreviewCheck()
    {
        if(levelpreviewcounter < levelpreviewtime)
            return;

        ((LevelPreviewWindow)levelPreview).UpdatePreviewData((LevelPin)character.currentPin);
        GameControl.control.savedPin = (LevelPin)character.currentPin;
        GameControl.control.AutosavePosition = character.transform.position;
        GameControl.control.savedCameraBackgroundColor = Camera.main.backgroundColor;
        levelPreview.Show();
    }

    public void LevelCounterReset()
    {
        levelpreviewcounter = 0f;
    }

    int GetShowing(out GUIWindow[] showing)
    {
        List<GUIWindow> showingCache = new List<GUIWindow>();
        foreach (GUIWindow window in All)
        {
            if(window.isShowing)
                showingCache.Add(window);
        }
        showing = showingCache.ToArray();
        return showing.Length;
    }

    public void Initialize(MapManager map, OverworldCharacter character)
    {
        this.map = map;
        this.character = character;
    }

    public void ToggleLevelPreview(LevelPin level)
    {
        ((LevelPreviewWindow)levelPreview).UpdatePreviewData(level);
        levelPreview.Toggle();
        selectedLevel = level;
    }
}
