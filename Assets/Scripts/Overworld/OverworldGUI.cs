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
    
    [HideInInspector] public OverworldCharacter character;
    [HideInInspector] public MapManager map;
    
    bool isAnyShowing => menuPrompt.isShowing || savePrompt.isShowing
            || loadPrompt.isShowing || levelPreview.isShowing;
   
    bool wasAnyShowing => menuPrompt.wasShowing || savePrompt.wasShowing
            || loadPrompt.wasShowing || levelPreview.wasShowing;

    void Start()
    {
        
    }

    void Update()
    {
        character.canMove = !isAnyShowing;
        if(character.currentPin is GatePin) return;

        if(GameInput.GetKeyDown("select"))
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
}
