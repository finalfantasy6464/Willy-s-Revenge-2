using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveFileRow : MonoBehaviour
{
    public Toggle toggle;
    public int saveSlot;
    public TextMeshProUGUI arenaScoreLabel;
    public TextMeshProUGUI completeLabel;
    public TextMeshProUGUI gateLabel;
    public TextMeshProUGUI challengeLabel;
    public TextMeshProUGUI goldenLabel;
    public TextMeshProUGUI timeLabel;
    const string EMPTY_TIME = "Not Set.";
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
        completeLabel.SetText($"{Mathf.FloorToInt(state.complete / 100)}%");
        gateLabel.SetText($"{Mathf.FloorToInt(GetFulfilled(state.destroyedgates) / 9)}%");
        challengeLabel.SetText($"{Mathf.FloorToInt(state.timer / 100)}%");
        goldenLabel.SetText($"{Mathf.FloorToInt(state.golden / 100)}%");
        timeLabel.SetText(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
    }

    public int GetFulfilled(List<bool> list)
    {
        int fulfilled = 0;
        foreach (bool value in list)
            if(value) fulfilled++;
        
        return fulfilled;
    }
}