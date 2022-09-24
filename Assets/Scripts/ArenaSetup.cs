using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArenaSetup : GUIWindow
{
    public Sprite[] skinSprites;
    public Sprite[] tailSprites;
    public Sprite[] levelIconSprites;
    public GameObject[] gridLayouts;
    public GameObject[] backgrounds;

    public Image skinIcon;
    public Image levelIcon;
    public PlayerController2021Arena arenaPlayer;
    
    public int skinIndex;
    public int levelIndex;
    public GameObject currentSelected;
    public GameObject currentSelectedCache;

    [Space]
    public Selectable skinButtonLeft;
    public Selectable skinButtonRight;
    public Selectable backgroundButtonLeft;
    public Selectable backgroundButtonRight;
   

    void Start()
    {
        InitializeUISelection();
    }

    void Update()
    {
        currentSelected = EventSystem.current.currentSelectedGameObject;
        if(GameInput.InputMapPressedDown["left"]() && currentSelectedCache == currentSelected)
        {
            if(currentSelected == skinButtonLeft.gameObject)
                SetSkinPrevious();
            else if(currentSelected == backgroundButtonLeft.gameObject)
                SetLevelPrevious();
        }
        else if(GameInput.InputMapPressedDown["right"]() && currentSelectedCache == currentSelected)
        {
            if(currentSelected == skinButtonRight.gameObject)
                SetSkinNext();
            else if(currentSelected == backgroundButtonRight.gameObject)
                SetLevelNext();
        }
        currentSelectedCache = currentSelected;
    }

    void InitializeUISelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
        currentSelected = firstSelected.gameObject;
    }

    public void SetSkinNext()
    {
        skinIndex = skinIndex == skinSprites.Length - 1 ? 0 : skinIndex + 1;
        skinIcon.sprite = skinSprites[skinIndex];
    }

    public void SetSkinPrevious()
    {
        skinIndex = skinIndex == 0 ? skinSprites.Length - 1 : skinIndex - 1;
        skinIcon.sprite = skinSprites[skinIndex];
    }

    public void SetLevelNext()
    {
        levelIndex = levelIndex == gridLayouts.Length - 1 ? 0 : levelIndex + 1;
        levelIcon.sprite = levelIconSprites[levelIndex];
    }

    public void SetLevelPrevious()
    {
        levelIndex = levelIndex == 0 ? levelIconSprites.Length - 1 : levelIndex - 1;
        levelIcon.sprite = levelIconSprites[levelIndex];
    }
}
