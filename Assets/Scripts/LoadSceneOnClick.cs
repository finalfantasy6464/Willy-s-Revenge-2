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
        if(sceneIndex == 101)
        {
            GameControl.control.StartCoroutine(GameControl.control.Setcamerasroutine());
        }
    }

	public void LoadProgress()
	{
        if(type == 0)
        {
            GameControl.control.Load();
        }

        if(type == 1)
        {
            GameControl.control.AutoLoad();
        }
	}

	public void LevelSelecter()
	{
		GameControl.control.returntoselect = !GameControl.control.returntoselect;
	}
}
