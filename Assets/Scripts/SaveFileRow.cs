using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SaveFileRow : MonoBehaviour
{
    public int saveSlot;
    [Header("Highlight Border")]
    public Image highlightBorder;
    public Color highlightedColor;
    public Color selectedColor;
    [Header("Labels")]
    public TextMeshProUGUI arenaScoreLabel;
    public TextMeshProUGUI completeLabel;
    public TextMeshProUGUI gateLabel;
    public TextMeshProUGUI challengeLabel;
    public TextMeshProUGUI goldenLabel;
    public TextMeshProUGUI timeLabel;
    const string EMPTY_TIME = "Not Set.";
    [Space]
    public bool isSelected;
    public bool isHighlighted;
    public bool isEmpty => timeLabel.text == EMPTY_TIME;

    public void SetEmpty()
    {
        arenaScoreLabel.SetText("0");
        completeLabel.SetText("0%");
        gateLabel.SetText("0%");
        challengeLabel.SetText("0%");
        goldenLabel.SetText("0%");
        timeLabel.SetText(EMPTY_TIME);
    }

    public void SetFromStatePreview(GameStatePreview preview)
    {
        Debug.Log("setting from state");
        arenaScoreLabel.SetText($"{preview.arenaScore}");
        completeLabel.SetText($"{Mathf.FloorToInt(preview.completedPercent * 100)}%");
        gateLabel.SetText($"{Mathf.FloorToInt(preview.destroyedPercent * 100)}%");
        challengeLabel.SetText($"{Mathf.FloorToInt(preview.challengePercent * 100)}%");
        goldenLabel.SetText($"{Mathf.FloorToInt(preview.goldenPercent * 100)}%");
        timeLabel.SetText(preview.formattedSaveTime);
    }

    public void SetFromControl(GameControl state)
    {
        Debug.Log("setting from control");
        arenaScoreLabel.SetText($"{state.ArenahighScore}");
        completeLabel.SetText($"{Mathf.FloorToInt(state.complete)}%");
        gateLabel.SetText($"{Mathf.FloorToInt(GetFulfilled(state.destroyedgates))}%");
        challengeLabel.SetText($"{Mathf.FloorToInt(state.timer)}%");
        goldenLabel.SetText($"{Mathf.FloorToInt(state.golden)}%");
        timeLabel.SetText(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
    }

    public float GetFulfilled(List<bool> list)
    {
        float fulfilled = 0;
        foreach (bool value in list)
            if(value) fulfilled += 11;
 
        if(fulfilled == 99f)
        {
            return 100f;
        }
        else
        {
            return fulfilled;
        }
    }

    public void SetHighlighted(bool value)
    {
        if(value && isSelected)
            highlightBorder.color = selectedColor;
        else if(value && !isSelected)
            highlightBorder.color = highlightedColor;
        else
            highlightBorder.color = Color.clear;
        
        isHighlighted = value;
    }
}