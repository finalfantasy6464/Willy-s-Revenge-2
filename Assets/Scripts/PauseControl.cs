using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseControl : MonoBehaviour
{
    public bool isGamePaused;
    GUIWindow menuPrompt;
    List<IPausable> pausables;
    EndLevelCanvas endCanvas;

    void Update()
    {
        if(endCanvas == null || endCanvas.gameObject.activeInHierarchy)
            return;
        
        if (GameInput.GetKeyDown("pause"))
            SetPause(!isGamePaused);
    }

    public void SetPause(bool value)
    {
        isGamePaused = value;
        if (value)
            menuPrompt.Show();
        else
            menuPrompt.Hide();

        foreach (IPausable pausable in pausables)
        {
            pausable.isPaused = value;
            if(value)
                pausable.OnPause();
            else
                pausable.OnUnpause();
        }
    }

    public void OnEnable()
    {
        Initialise();
    }

    public void Initialise()
    {
        menuPrompt = GameObject.Find("QuitPanel").GetComponent<GUIWindow>();
        endCanvas = Resources.FindObjectsOfTypeAll<EndLevelCanvas>()[0];
        RegeneratePausables();  
        isGamePaused = false;
    }

    void RegeneratePausables()
    {
        pausables = new List<IPausable>();
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach(GameObject root in rootGameObjects)
        {
            foreach(IPausable pausable in root.GetComponentsInChildren<IPausable>())
            {
                pausables.Add(pausable);
            }
        }
    }
}
