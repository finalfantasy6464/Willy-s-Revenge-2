using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePrompt : GUIWindow
{
    public Button button;
    public int toSave;
    
    public void SetSlot(int saveSlot)
    {
        toSave = saveSlot;
    }

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() => GameControl.control.Save(toSave));
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(() => GameControl.control.Save(toSave));
    }
}
