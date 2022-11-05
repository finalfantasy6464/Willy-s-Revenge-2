using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public GUIWindow gatepinPreview;
    public GUIWindow optionsPanel;
    public GUIWindow saveLoadPanel;
    public GUIWindow saveLoadConfirmationPanel;
    public GUIWindow quitPanel;
    public GUIWindow tutorial_1;
    public GUIWindow tutorial_2;
    public GUIWindow tutorial_3;

    private float levelpreviewcounter;
    private float levelpreviewtime = 0.1f;

    public GUIWindow[] All => new GUIWindow[]
    {
        pauseMenu, levelPreview, gatepinPreview, optionsPanel, saveLoadPanel,
        saveLoadConfirmationPanel, quitPanel, tutorial_1, tutorial_2, tutorial_3
    };
    
    public OverworldPlayer character;
    [HideInInspector]
    public MapManager map;

    bool isAnyShowing => All.Any(w => w.isShowing);
    bool wasAnyShowing => All.Any(w => w.wasShowing);
    
    bool isTutorialShowing => tutorial_1.isShowing || tutorial_2.isShowing || tutorial_3.isShowing;
    bool wasTutorialShowing => tutorial_1.wasShowing || tutorial_2.wasShowing || tutorial_3.wasShowing;


    void Start()
    {
        character.OpenMenu += OpenMenu;
        character.CloseMenu += CancelMenu;
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name.Contains("2021"))
            return;

        if(levelpreviewcounter < levelpreviewtime)
            levelpreviewcounter += Time.deltaTime;
    }

    void OpenMenu()
    {
        if (GetShowing(out GUIWindow[] showing) > 0)
        {
            foreach (GUIWindow window in showing)
            {
                if (window == levelPreview || window == gatepinPreview)
                    return;
                if (window == saveLoadPanel)
                    ((SaveLoadPanel)window).ProcessCloseAll();
                window.Close();
                character.canMove = true;
            }
            return;
        }
        else
        {
            character.canMove = false;
            pauseMenu.Open();
            return;
        }
    }

    void CancelMenu()
    {
        if (GetShowing(out GUIWindow[] showing) > 0)
        {
            foreach (GUIWindow window in showing)
            {
                if (window == levelPreview || window == gatepinPreview)
                    return;

                else if (window == saveLoadPanel)
                {
                    ((SaveLoadPanel)window).ProcessCancelInput();
                    character.canMove = true;
                    return;
                }
                else if (window == optionsPanel)
                {
                    ((OptionsPanel)window).ProcessCancelInput();
                    character.canMove = true;
                    return;
                }
                else if (window is GUIPrompt prompt)
                    prompt.navigationParent.Open();
                window.Close();
                character.canMove = true;
            }
        }
    }

    private void LevelPreviewCheck()
    {
        if(levelpreviewcounter < levelpreviewtime)
            return;
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

    public void Initialize(MapManager map, OverworldPlayer character)
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



