using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationUpdater : MonoBehaviour
{
    int ID;

    // Start is called before the first frame update
    void Start()
    {
        ID = SceneManager.GetActiveScene().buildIndex;
        GameControl.control.levelID = ID;
    }
}
