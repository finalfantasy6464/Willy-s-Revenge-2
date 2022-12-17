using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelPreviewWindow : GUIWindow
{
    public Sprite[] backgroundImageSprites;
    public Image currentBackgroundImage;
    public TextMeshProUGUI levelNameLabel;
    public TextMeshProUGUI levelParLabel;
    public Image snapshot;
    TimeSpan parSpan;

    public void UpdatePreviewData(OverworldLevelPin pin)
    {
        currentBackgroundImage.sprite = backgroundImageSprites[pin.worldIndex - 1];
        levelNameLabel.text = $"{pin.levelDisplayName}";
        parSpan = TimeSpan.FromSeconds(pin.parTime);
        levelParLabel.text = "Par Time: " + parSpan.ToString(@"mm\:ss");
        snapshot.sprite = pin.levelPreviewSprite;
    }
}
