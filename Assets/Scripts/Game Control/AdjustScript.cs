using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdjustScript : MonoBehaviour
{

    private void Awake()
    {
    }

    void OnGUI()
	{
		
		if (GUI.Button (new Rect (800, 260, 150, 30), "Save")) {
			GameControl.control.Save ();
		}

		if (GUI.Button (new Rect (800, 300, 150, 30), "Load")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GameControl.control.Load ();
		}
	}
}
