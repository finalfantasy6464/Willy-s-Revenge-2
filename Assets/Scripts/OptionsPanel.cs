using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsPanel : GUIWindow
{
    public ScriptablePlayerSettings settings;
    public TMP_Dropdown resOptions;
    public TMP_Dropdown screenOptions;

    public override void Hide()
    {
        base.Hide();
        settings.SaveToDisk();
    }

    public void ProcessCancelInput()
    {
        foreach(Transform t in resOptions.transform)
        {
            if (t.gameObject.name == "Dropdown List")
            {
                resOptions.Hide();
                return;
            }
        }
        foreach (Transform t in screenOptions.transform)
        {
            if (t.gameObject.name == "Dropdown List")
            {
                screenOptions.Hide();
                return;
            }
        }
        Close();
    }
}
