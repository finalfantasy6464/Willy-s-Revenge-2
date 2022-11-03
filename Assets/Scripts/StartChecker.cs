using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartChecker : MonoBehaviour
{
    public ScriptablePlayerSettings settings;

    void RunGameControlCheck()
    {
        if (!File.Exists(Application.persistentDataPath + "/config.ini"))
        {
            settings.CreateNew();
            SceneManager.LoadScene(104);
        }
        else
        {
            settings.TryLoadFromDisk();
            SceneManager.LoadScene(104);
        }


        /*
        if (File.Exists(Application.persistentDataPath + "/Save_Auto.wr2"))
        {
            //GameControl.control.AutoLoad();
            //CheckStarted();
            SceneManager.LoadScene(104);
        }
        else
        {
            //CheckStarted();
            SceneManager.LoadScene(101);
        }
        */

    }
    void CheckStarted()
    {
        /*
        if(GameControl.control.InitialGameStarted == true)
        {
            SceneManager.LoadScene(101);
        }
        else
        {
            SceneManager.LoadScene(104);
        }
        */
    }
}
