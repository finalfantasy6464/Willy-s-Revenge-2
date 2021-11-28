using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPrompt : GUIWindow
{
    public Button button;
    public int toLoad;

    public void SetSlot(int saveSlot)
    {
        toLoad = saveSlot;
    }

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() => GameControl.control.Load(toLoad));
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(() => GameControl.control.Load(toLoad));
    }
}
