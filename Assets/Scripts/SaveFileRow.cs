using System;
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
        arenaScoreLabel.SetText($"{preview.arenaScore}");
        completeLabel.SetText($"{Mathf.FloorToInt(preview.completedPercent)}%");
        gateLabel.SetText($"{Mathf.FloorToInt(preview.destroyedPercent)}%");
        challengeLabel.SetText($"{Mathf.FloorToInt(preview.challengePercent)}%");
        goldenLabel.SetText($"{Mathf.FloorToInt(preview.goldenPercent)}%");
        timeLabel.SetText(preview.formattedSaveTime);
    }

    public void SetFromControl(GameControl state)
    {
        Debug.Log("setting from control");
        arenaScoreLabel.SetText($"{state.ArenahighScore}%");
        completeLabel.SetText($"{Mathf.FloorToInt(state.complete / state.completedlevels.Count)}%");
        gateLabel.SetText($"{Mathf.FloorToInt(state.complete / state.completedlevels.Count)}%");
        challengeLabel.SetText($"{Mathf.FloorToInt(state.complete / state.completedlevels.Count)}%");
        goldenLabel.SetText($"{Mathf.FloorToInt(state.complete / state.completedlevels.Count)}%");
        timeLabel.SetText(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
    }
}