using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamepadBackEnabler : MonoBehaviour
{
    Button button;

    public int buttontype;

    public bool selectionLock = true;

    public CanvasGroup canvas;

    GamepadBackEnabler[] localenablers;

    public void Start()
    {
        button = GetComponent<Button>();

        localenablers = FindObjectsOfType<GamepadBackEnabler>();
    }

    public void Update()
    {
        // if(canvas.alpha == 1)
        // {
        //     if (buttontype == 0)
        //     {
        //         if (GameInput.InputMapPressedDown["cancel"]() && selectionLock == false)
        //         {
        //             foreach (GamepadBackEnabler enabler in localenablers)
        //             {
        //                 enabler.selectionLock = true;
        //             }
        //             button.onClick.Invoke();
        //         }
        //     }
        //     else if (buttontype == 1)
        //     {
        //         if (GameInput.InputMapPressedDown["select"]() && selectionLock == false)
        //         {
        //             foreach (GamepadBackEnabler enabler in localenablers)
        //             {
        //                 enabler.selectionLock = true;
        //             }
        //             button.onClick.Invoke();
        //         }
        //     }
        // }  
    }
}