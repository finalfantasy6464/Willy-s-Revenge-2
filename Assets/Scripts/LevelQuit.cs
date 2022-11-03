using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelQuit : GUIWindow
{ 
    public Button noButton;
    PauseControl pause;

    // Start is called before the first frame update
    void Start()
    {
        if (pause == null)
            pause = FindObjectOfType<PauseControl>();

        noButton.onClick.AddListener(() => pause.SetPause(false));
    }

    private void OnDisable()
    {
        noButton.onClick.RemoveListener(() => pause.SetPause(false));
    }
}
