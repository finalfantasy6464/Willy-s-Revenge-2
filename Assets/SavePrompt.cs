using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePrompt : GUIWindow
{
    public Button saveButton;

    // Start is called before the first frame update
    void Start()
    {
        saveButton.onClick.AddListener(() => GameControl.control.Save());
    }

    private void OnDisable()
    {
        saveButton.onClick.RemoveListener(() => GameControl.control.Save());
    }
}
