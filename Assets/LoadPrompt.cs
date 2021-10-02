using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPrompt : GUIWindow
{
    public Button loadButton;

    // Start is called before the first frame update
    void Start()
    {
        loadButton.onClick.AddListener(() => GameControl.control.Load());
    }

    private void OnDisable()
    {
        loadButton.onClick.RemoveListener(() => GameControl.control.Load());
    }
}
