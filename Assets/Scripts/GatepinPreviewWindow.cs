using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GatepinPreviewWindow : GUIWindow
{
    public TextMeshProUGUI GatepinIndex;
    public TextMeshProUGUI completerequired;
    public Button goButton;

    GatePin selectedGatepin;

    public void UpdatePreviewData(GatePin pin)
    {
        GatepinIndex.text = "Barrier " + $"{pin.gatepinIndex}";
        completerequired.text = "Completed Requirement: " + $"{pin.completerequired}";
        selectedGatepin = pin;
        if (GameControl.control.complete >= pin.completerequired)
        {
            goButton.interactable = true;
        }
        else
        {
            goButton.interactable = false;
        }
    }

    public void LockCheck()
    {
        selectedGatepin.LockCheck();
    }
}
