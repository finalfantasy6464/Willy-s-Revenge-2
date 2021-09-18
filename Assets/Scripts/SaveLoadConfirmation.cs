using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadConfirmation : MonoBehaviour
{
    public Button Savebutton;
    public Button Loadbutton;

    private void Start()
    {
        Savebutton.onClick.AddListener(localSave);
        Loadbutton.onClick.AddListener(localLoad);
    }

    private void OnDisable()
    {
        Savebutton.onClick.RemoveListener(localSave);
        Loadbutton.onClick.RemoveListener(localLoad);
    }

    private void localSave()
    {
        FindObjectOfType<GameControl>().Save();
    }

    private void localLoad()
    {
        FindObjectOfType<GameControl>().Load();
    }
}
