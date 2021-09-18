using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    public int type = 0;

	public void LoadByIndex (int sceneIndex)
	{
		SceneManager.LoadScene (sceneIndex);
        if (sceneIndex == 101)
        {
            GameControl.control.StartCoroutine(GameControl.control.Setcamerasroutine());

			if(type == 0)
            {
                OverworldLock.loadlocked = false;
            }
            if (type == 1)
            {
                OverworldLock.Autoloadlocked = false;
            }
        }
    }

	public void LevelSelecter()
	{
		GameControl.control.returntoselect = !GameControl.control.returntoselect;
	}
}
