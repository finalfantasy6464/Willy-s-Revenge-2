using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    int Leveltoload;

   public void LoadLevelNow()
    {
        Leveltoload = GameControl.control.levelID;
        SceneManager.LoadScene(Leveltoload);
    }
}
