using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseControl : MonoBehaviour
{
    public bool isGamePaused;
    CanvasGroup menuPromptGroup;
    List<IPausable> pausables;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPause(!isGamePaused);
        }
    }

    void SetPause(bool value)
    {
        menuPromptGroup.alpha = value ? 1f : 0f;
        menuPromptGroup.interactable = value;
        menuPromptGroup.blocksRaycasts = value;

        foreach (IPausable pausable in pausables)
        {
            pausable.isPaused = value;
            if(value)
                pausable.OnPause();
            else
                pausable.OnUnpause();
        }
    }

    public void OnLevelLoaded()
    {
        // Change to ElementGUI and use Show/Hide, also helps with usability
        menuPromptGroup = FindObjectOfType<SelectOnInput>().GetComponent<CanvasGroup>();
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
