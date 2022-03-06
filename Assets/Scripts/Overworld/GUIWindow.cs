using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIWindow : GUIElement
{
    [SerializeField] protected Selectable firstSelected; //Insert into in-between GUIWindow class if requirements increase.

    public override void Show()
    {
        firstSelected?.Select();
        base.Show();
    }
    
    public override void Show(float time)
    {
        firstSelected?.Select();
        base.Show(time);
    }
}
