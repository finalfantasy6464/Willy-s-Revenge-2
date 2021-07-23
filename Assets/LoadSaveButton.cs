using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadSaveButton : MonoBehaviour
{
    public Button loadSaveButton;

    public int SaveType = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (SaveType == 0)
        {
            loadSaveButton.interactable = (File.Exists(Application.persistentDataPath + "/playersave.wr2") || (File.Exists(Application.persistentDataPath + "/autosave.wr2")));
        }

        if (SaveType == 1)
        {
            loadSaveButton.interactable = (File.Exists(Application.persistentDataPath + "/playersave.wr2"));
        }

        if (SaveType == 2)
        {
            loadSaveButton.interactable = (File.Exists(Application.persistentDataPath + "/autosave.wr2"));
        }
    }
}
