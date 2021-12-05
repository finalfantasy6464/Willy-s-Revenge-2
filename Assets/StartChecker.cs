using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartChecker : MonoBehaviour
{
    ScriptablePlayerSettings settings;
    void Start()
    {
        if(!File.Exists(Application.persistentDataPath + "/config.ini"))
        {
            settings.CreateNew();
        }

        if(File.Exists(Application.persistentDataPath + "/Save_Auto.wr2"))
        {
            GameControl.control.AutoLoad();
            CheckStarted();
        }
    }

    void CheckStarted()
    {
        if(GameControl.control.InitialGameStarted == true)
        {
            SceneManager.LoadScene(101);
        }
    }
}
