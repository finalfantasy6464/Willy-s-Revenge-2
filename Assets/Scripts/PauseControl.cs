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

    public void Initialise()
    {
        menuPrompt = GameObject.Find("QuitPanel").GetComponent<GUIWindow>();
        endCanvas = Resources.FindObjectsOfTypeAll<EndLevelCanvas>()[0];
        RegeneratePausables();  
        isGamePaused = false;
    }

    void Update()
    {
        if(endCanvas == null || endCanvas.gameObject.activeInHierarchy)
            return;
        
        if (GameInput.GetKeyDown("pause"))
            SetPause(!isGamePaused);

        if(GameInput.GetKeyDown("cancel") && isGamePaused)
        {
            SetPause(!isGamePaused);
        }
    }

    public static bool TryAddPausable(GameObject pausableObject)
    {
        bool exists = false;
        if(instance == null)
                instance = FindObjectOfType<PauseControl>();

        if(pausableObject.TryGetComponent<IPausable>(out IPausable pausable))
        {
            foreach (MonoBehaviour mono in ((MonoBehaviour)pausable).GetComponents<MonoBehaviour>())
            {
                if(mono is IPausable)
                    instance.pausables.Add(pausable);
            }
            exists = true;
        }
        
        return exists;
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
            //Debug.Log($"{(pausable as MonoBehaviour).gameObject.name} .paused:  {pausable.isPaused}");
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
