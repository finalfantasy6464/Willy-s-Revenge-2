using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
        EndLevelCanvas[] levelCanvas = Resources.FindObjectsOfTypeAll<EndLevelCanvas>();
        endCanvas = levelCanvas.Length > 0 ? levelCanvas[0] : null;
        RegeneratePausables();  
        isGamePaused = false;
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //if(Application.isEditor)
        //{
        //    EditorApplication.playModeStateChanged += UpdatePlayState;
        //}
    }

    void LateUpdate()
    {
        if(endCanvas == null || endCanvas.canvasGroup.alpha == 1)
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
        if(instance == null || instance.pausables == null || instance.quitting)
            return false;
            
        if(pausableObject.TryGetComponent<IPausable>(out IPausable pausable))
        {
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

    //void UpdatePlayState(PlayModeStateChange state)
    //{
    //    if(state == PlayModeStateChange.ExitingPlayMode)
    //        quitting = true;
   // }
}
