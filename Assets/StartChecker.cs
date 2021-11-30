using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
