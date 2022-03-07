using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPanel : GUIWindow
{
    public ScriptablePlayerSettings settings;
    public override void Hide()
    {
        base.Hide();
        settings.SaveToDisk();
    }
}
