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
    static PauseControl instance;
    bool quitting;

    void Update()
    {
        if(endCanvas == null || endCanvas.gameObject.activeInHierarchy)
            return;
        
        if (GameInput.GetKeyDown("pause"))
            SetPause(!isGamePaused);
    }

    public static bool TryAddPausable(GameObject pausableObject)
    {
        if(pausableObject.TryGetComponent<IPausable>(out IPausable pausable))
        {
            if(instance == null)
                instance = FindObjectOfType<PauseControl>();
            instance.pausables.Add(pausable);
            return true;
        }
        
        return false;
    }

    public static bool TryRemovePausable(GameObject pausableObject)
    {
        if(instance.quitting) return false;
        if(pausableObject.TryGetComponent<IPausable>(out IPausable pausable))
        {
            if(instance == null)
                instance = FindObjectOfType<PauseControl>();
            instance.pausables.Remove(pausable);
            return true;
        }
        return false;
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

    void OnApplicationQuit()
    {
        quitting = true;
    }
}
