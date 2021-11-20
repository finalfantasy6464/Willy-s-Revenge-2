using System;
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
    [Header("Live Data")]
    public LevelPin selectedLevel;

    [Header("GUI Elements")]
    public GUIWindow menuPrompt;
    public GUIWindow savePrompt;
    public GUIWindow loadPrompt;
    public GUIWindow levelPreview;
    public GUIWindow optionsPanel;
    public GUIWindow tutorial_1;
    public GUIWindow tutorial_2;
    public GUIWindow tutorial_3;

    public Text completionText;
    
    [HideInInspector] public OverworldCharacter character;
    [HideInInspector] public MapManager map;
    
    bool isAnyShowing => menuPrompt.isShowing || savePrompt.isShowing
            || loadPrompt.isShowing || levelPreview.isShowing || optionsPanel.isShowing;

    bool isLevelPreviewValid => !(menuPrompt.isShowing || savePrompt.isShowing
            || loadPrompt.isShowing || optionsPanel.isShowing);
   
    bool wasAnyShowing => menuPrompt.wasShowing || savePrompt.wasShowing
            || loadPrompt.wasShowing || levelPreview.wasShowing || optionsPanel.wasShowing;

    bool isTutorialShowing => tutorial_1.isShowing || tutorial_2.isShowing || tutorial_3.isShowing;
    bool wasTutorialShowing => tutorial_1.wasShowing || tutorial_2.wasShowing || tutorial_3.wasShowing;

    void Start()
    {
        UpdateText();
    }

    void Update()
    {
        character.canMove = !isAnyShowing;
        if(character.currentPin is GatePin) return;

        if(GameInput.GetKeyDown("select") && isLevelPreviewValid && !isTutorialShowing)
        {
            if (!wasAnyShowing && !character.isMoving)
            {
                ((LevelPreviewWindow)levelPreview).UpdatePreviewData((LevelPin)character.currentPin);
                GameControl.control.savedPin = (LevelPin)character.currentPin;
                levelPreview.Show();
            }
            else if (levelPreview.isShowing)
                 map.LoadLevelFromCurrentPin();
        }
        
        if(GameInput.GetKeyDown("pause"))
        {
            if(optionsPanel.isShowing)
            {
                optionsPanel.Hide();
                return;
            }

            if(loadPrompt.isShowing)
            {
                optionsPanel.Hide();
                return;
            }

            if(savePrompt.isShowing)
            {
                optionsPanel.Hide();
                return;
            }

            if(isTutorialShowing)
            {
                tutorial_1.Hide();
                tutorial_2.Hide();
                tutorial_3.Hide();
                return;
            }

            if(!levelPreview.isShowing)
                menuPrompt.Toggle();
            else
                levelPreview.Hide();
        }
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

    public void UpdateText()
    {
        completionText.text = "Completion: " + GameControl.control.completionPercent.ToString() + "%";
    }
}
