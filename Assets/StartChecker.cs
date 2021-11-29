using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameControl.control.AutoLoad();
        CheckStarted();
    }

    void CheckStarted()
    {
        if(GameControl.control.InitialGameStarted == true)
        {
            SceneManager.LoadScene(101);
        }

        /*When loading the scene, needs to put the player at the right location, unsure if currently does or not.*/
    }
}
