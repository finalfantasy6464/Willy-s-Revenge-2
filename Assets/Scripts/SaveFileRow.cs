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

    }

    public void SetFromControl(GameControl state)
    {
        throw new NotImplementedException();
    }
}