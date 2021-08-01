using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadConfirmation : MonoBehaviour
{
    public Button Savebutton;
    public Button Loadbutton;

    private void OnEnable()
    {
        Savebutton.onClick.AddListener(()=> GameControl.control.Save());
        Loadbutton.onClick.AddListener(() => GameControl.control.Load());
    }

    private void OnDisable()
    {
        Savebutton.onClick.RemoveListener(() => GameControl.control.Save());
        Loadbutton.onClick.RemoveListener(() => GameControl.control.Load());
    }
}
