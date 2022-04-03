using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelPreviewWindow : GUIWindow
{
    public TextMeshProUGUI levelNameLabel;
    public TextMeshProUGUI levelParLabel;
    public Image snapshot;
    public Image completeEmblem;
    public Image timeEmblem;
    public Sprite[] emblemSprites; // 0 is None, 1 is Complete, 2 is Gold, 3 is Time
    TimeSpan parSpan;

    ///<Summary>
    /// Updates the display information in the window based on the current pin's data
    ///</Summary>
    public void UpdatePreviewData(LevelPin pin)
    {
        levelNameLabel.text = $"{pin.levelDisplayName}";
        parSpan = TimeSpan.FromSeconds(pin.parTime);
        levelParLabel.text = "Par Time: " + parSpan.ToString(@"mm\:ss");
        snapshot.sprite = pin.levelPreviewSprite;
        
        if(pin.goldChallenge) completeEmblem.sprite = emblemSprites[2];
        else if(pin.complete) completeEmblem.sprite = emblemSprites[1];
        else completeEmblem.sprite = emblemSprites[0];

        timeEmblem.sprite = pin.timeChallenge ? emblemSprites[3] : emblemSprites[0];
    }
}
